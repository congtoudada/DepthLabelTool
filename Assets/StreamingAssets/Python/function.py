import argparse
import json

from PIL import Image
import numpy as np
import os
import cv2
import math
import shutil

def make_parser():
    parser = argparse.ArgumentParser("Export Depth PNG!")
    parser.add_argument("--json_path", type=str, default=r'E:\Practice\Unity\Material\DUT\label_toolkit_demo\1020\parking_plot\20231020160928\temp\00001_left_ccd.json')
    # 0:可视化 1:追加 2:检验 3:追加到模板
    parser.add_argument("--run_mode", type=int, default=0)
    return parser

def load_image(file_path):
    img = np.array(Image.open(file_path))  # (960,1280,3), RGB
    return img

def draw_projected_box3d(image, corners3d, color=(0, 0, 255), thickness=1):
    ''' Draw 3d bounding box in image
    input:
        image: RGB image
        corners3d: (8,3) array of vertices (in image plane) for the 3d box in following order:
            1 -------- 0
           /|         /|
          2 -------- 3 .
          | |        | |
          . 5 -------- 4
          |/         |/
          6 -------- 7

    '''
    image = image.copy()
    corners3d = corners3d.astype(np.int32)
    for k in range(0, 4):
        i, j = k, (k + 1) % 4
        cv2.line(image, (corners3d[i, 0], corners3d[i, 1]), (corners3d[j, 0], corners3d[j, 1]), color, thickness, lineType=cv2.LINE_AA)
        i, j = k + 4, (k + 1) % 4 + 4
        cv2.line(image, (corners3d[i, 0], corners3d[i, 1]), (corners3d[j, 0], corners3d[j, 1]), color, thickness, lineType=cv2.LINE_AA)
        i, j = k, k + 4
        cv2.line(image, (corners3d[i, 0], corners3d[i, 1]), (corners3d[j, 0], corners3d[j, 1]), color, thickness, lineType=cv2.LINE_AA)
    return image

def vis_object(data_dir, sample, obj):
    vis_path = os.path.join(data_dir, 'vis')
    if not os.path.exists(vis_path):
        os.mkdir(vis_path)

    # load images
    img_left  = load_image(os.path.join(data_dir, 'images', '%s_left_ccd.bmp' % sample))  # (960,1280,3), RGB
    img_right = load_image(os.path.join(data_dir, 'images', '%s_right_ccd.bmp' % sample))  # (960,1280,3), RGB
    img_left  = cv2.cvtColor(img_left, cv2.COLOR_RGB2BGR, img_left)
    img_right = cv2.cvtColor(img_right, cv2.COLOR_RGB2BGR, img_right)

    # label
    corners3d = obj.generate_corners3d()                                        # 左相机坐标系 (8,3)
    corners3d_left, _  = calib.rect_to_img_left(corners3d)                      # 左图像坐标系 (8,2)
    corners3d_right, _ = calib.rect_to_img_right2(corners3d)                    # 右图像坐标系 (8,2)
    
    # vis 3d bbox
    img_left_label  = draw_projected_box3d(img_left,  corners3d_left)
    img_right_label = draw_projected_box3d(img_right, corners3d_right)
    cv2.imwrite(os.path.join(data_dir, 'vis', '%s_left.png' % (sample)), img_left_label)
    cv2.imwrite(os.path.join(data_dir, 'vis', '%s_right.png' % (sample)), img_right_label)

def write_object(data_dir, sample, obj):
    label_path = os.path.join(data_dir, 'label_2')
    if not os.path.exists(label_path):
        os.mkdir(label_path)
    
    label_file_path = os.path.join(label_path, '%s.txt' % sample)
    if not os.path.exists(label_file_path):                           # 当前样本 未标注
        if os.path.exists(os.path.join(label_path, 'template.txt')):  # 当前样本 有template
            shutil.copy('label_2//template.txt', 'label_2//%s.txt' % sample)
    with open(label_file_path, 'a+') as f:
        f.write('%s 0.00 0 %f %f %f %f %f %f %f %f %f %f %f %f \n' % \
                    (obj.cls_type, obj.alpha, obj.box2d[0], obj.box2d[1], obj.box2d[2], obj.box2d[3], \
                    obj.h, obj.w, obj.l, obj.pos[0], obj.pos[1], obj.pos[2], obj.ry))

def get_objects_from_label(label_file):
    with open(label_file, 'r') as f:
        lines = f.readlines()
    
    objects = []
    for line in lines:
        obj = Object3d()
        obj.init_from_kitti(line)
        objects.append(obj)
    return objects

class Object3d(object):
    def __init__(self):
        pass
    
    def init_from_hand(self, label):
        self.h = float(label[0])
        self.w = float(label[1])
        self.l = float(label[2])
        self.pos = np.array((float(label[3]), float(label[4]), float(label[5])), dtype=np.float32)
        self.ry = float(label[6])
        if len(label) > 7:
            self.box2d = np.array((float(label[7]), float(label[8]), float(label[9]), float(label[10])), dtype=np.float32)
            self.alpha = calib.ry2alpha(self.ry, (float(label[7]) + float(label[9])) / 2)
    
    def init_from_kitti(self, line):
        label = line.strip().split(' ')
        self.cls_type = label[0]
        self.trucation = float(label[1])
        self.occlusion = float(label[2])
        self.alpha = float(label[3])
        self.box2d = np.array((float(label[4]), float(label[5]), float(label[6]), float(label[7])), dtype=np.float32)
        self.h = float(label[8])
        self.w = float(label[9])
        self.l = float(label[10])
        self.pos = np.array((float(label[11]), float(label[12]), float(label[13])), dtype=np.float32)
        self.dis_to_cam = np.linalg.norm(self.pos)
        self.ry = float(label[14])
        self.score = float(label[15]) if label.__len__() == 16 else -1.0
    
    def generate_corners3d(self):
        """
        generate corners3d representation for this object
        :return corners_3d: (8, 3) corners of box3d in camera coord
        """
        l, h, w = self.l, self.h, self.w
        x_corners = [l / 2, l / 2, -l / 2, -l / 2, l / 2, l / 2, -l / 2, -l / 2]
        y_corners = [0, 0, 0, 0, -h, -h, -h, -h]
        z_corners = [w / 2, -w / 2, -w / 2, w / 2, w / 2, -w / 2, -w / 2, w / 2]

        R = np.array([[np.cos(self.ry), 0, np.sin(self.ry)],
                      [0, 1, 0],
                      [-np.sin(self.ry), 0, np.cos(self.ry)]])
        corners3d = np.vstack([x_corners, y_corners, z_corners])  # (3, 8)
        corners3d = np.dot(R, corners3d).T
        corners3d = corners3d + self.pos
        return corners3d

class Calibration(object):
    def __init__(self):
        # 校准后
        self.P_left = np.array([
            [1239.901799,            0, 717.9559837312746, 0],
            [           0, 1239.901799, 535.4391188237089, 0],
            [           0,           0,                 1, 0]])

        self.P_right = np.array([
            [1239.901799,            0, 717.9559837312746, 0],
            [           0, 1239.901799, 535.1436698237089, 0],
            [           0,           0,                 1, 0]])

        # 外参 融入 变换矩阵
        self.P2 = np.array([
            [1239.901799,            0, 717.9559837312746, 0],
            [           0, 1239.901799, 535.4391188237089, 0],
            [           0,           0,                 1, 0]])
        self.P3 = np.array([
            [1239.901799,            0, 717.9559837312746, -148.78821588],
            [           0, 1239.901799, 535.1436698237089, 0],
            [           0,           0,                 1, 0]])

        self.cu = self.P2[0, 2]
        self.cv = self.P2[1, 2]
        self.fu = self.P2[0, 0]
        self.fv = self.P2[1, 1] 
        self.tx = self.P2[0, 3] / (-self.fu)
        self.ty = self.P2[1, 3] / (-self.fv)

    def cart_to_hom(self, pts):
        """
        :param pts: (N, 3 or 2)
        :return pts_hom: (N, 4 or 3)
        """
        pts_hom = np.hstack((pts, np.ones((pts.shape[0], 1), dtype=np.float32)))
        return pts_hom
    
    def rect_to_img_left(self, pts_rect):
        """
        :param pts_rect: (N, 3)
        :return pts_img: (N, 2)
        """
        pts_rect_hom = self.cart_to_hom(pts_rect)  # (N,3) -> (N,4)
        pts_2d_hom = np.dot(pts_rect_hom, self.P_left.T)  # (N,4)*(4,3) -> (N,3)
        pts_img = (pts_2d_hom[:, 0:2].T / pts_rect_hom[:, 2]).T  # (N,2)/(N,1) -> (N,2)
        pts_rect_depth = pts_2d_hom[:, 2] - self.P_left.T[3, 2]  # depth in rect camera coord
        return pts_img, pts_rect_depth

    def rect_to_img_right2(self, pts_rect):  # P3
        """
        :param pts_rect: (N, 3)
        :return pts_img: (N, 2)
        """
        pts_rect_hom = self.cart_to_hom(pts_rect)  # (N,3) -> (N,4)
        pts_2d_hom = np.dot(pts_rect_hom, self.P3.T)  # (N,4)*(4,3) -> (N,3)
        pts_img = (pts_2d_hom[:, 0:2].T / pts_rect_hom[:, 2]).T  # (N,2)/(N,1) -> (N,2)
        pts_rect_depth = pts_2d_hom[:, 2] - self.P3.T[3, 2]  # depth in rect camera coord
        return pts_img, pts_rect_depth

    def alpha2ry(self, alpha, u):
        """
        Get rotation_y by alpha + theta - 180
        alpha : Observation angle of object, ranging [-pi..pi]
        x : Object center x to the camera center (x-W/2), in pixels
        rotation_y : Rotation ry around Y-axis in camera coordinates [-pi..pi]
        """
        ry = alpha + np.arctan2(u - self.cu, self.fu)

        if ry > np.pi:
            ry -= 2 * np.pi
        if ry < -np.pi:
            ry += 2 * np.pi

        return ry

    def ry2alpha(self, ry, u):
        alpha = ry - np.arctan2(u - self.cu, self.fu)

        if alpha > np.pi:
            alpha -= 2 * np.pi
        if alpha < -np.pi:
            alpha += 2 * np.pi

        return alpha

def f_vis(data_dir, sample, height, width, length, x_center, y_bottom, z_center, ry_ind, left, top, right, bottom, cls_id):
    ry_dic = {0: math.radians(90), 1: math.radians(179.99), 2: math.radians(0.01)}
    ry = ry_dic[ry_ind]
    if ry_ind == 1 or ry_ind == 2:
        z_center += width / 2
    elif ry_ind == 0:
        z_center += length / 2
    else:
        raise KeyError()
    obj = Object3d()
    obj.init_from_hand([height, width, length, x_center, y_bottom, z_center, ry, left, top, right, bottom])   # 左相机坐标系
    cls_name = {0: 'Car', 1: 'Pedestrian'}
    obj.cls_type = cls_name[cls_id]
    vis_object(data_dir, sample, obj)

def f_write(data_dir, sample, height, width, length, x_center, y_bottom, z_center, ry_ind, left, top, right, bottom, cls_id):
    ry_dic = {0: math.radians(90), 1: math.radians(179.99), 2: math.radians(0.01)}
    ry = ry_dic[ry_ind]
    if ry_ind == 1 or ry_ind == 2:
        z_center += width / 2
    elif ry_ind == 0:
        z_center += length / 2
    else:
        raise KeyError()
    obj = Object3d()
    obj.init_from_hand([height, width, length, x_center, y_bottom, z_center, ry, left, top, right, bottom])   # 左相机坐标系
    cls_name = {0: 'Car', 1: 'Pedestrian'}
    obj.cls_type = cls_name[cls_id]
    write_object(data_dir, sample, obj)

def f_verify(data_dir, sample):
    label_path = os.path.join(data_dir, 'label_2')
    objects = get_objects_from_label(os.path.join(label_path, sample+'.txt'))

    # load images (image_2 && image_3 目录)
    img_left  = load_image(os.path.join(data_dir, 'images', '%s_left_ccd.bmp' % sample))  # (960,1280,3), RGB
    img_right = load_image(os.path.join(data_dir, 'images', '%s_right_ccd.bmp' % sample))  # (960,1280,3), RGB
    img_left  = cv2.cvtColor(img_left, cv2.COLOR_RGB2BGR, img_left)
    img_right = cv2.cvtColor(img_right, cv2.COLOR_RGB2BGR, img_right)

    # load label && vis 3d bbox
    for i in range(len(objects)):
        corners3d = objects[i].generate_corners3d()                                 # 左相机坐标系 (8,3)
        corners3d_left, _  = calib.rect_to_img_left(corners3d)                      # 左图像坐标系 (8,2)
        corners3d_right, _ = calib.rect_to_img_right2(corners3d)                    # 右图像坐标系 (8,2)
        
        # vis 3d bbox
        img_left  = draw_projected_box3d(img_left,  corners3d_left)
        img_right = draw_projected_box3d(img_right, corners3d_right)
        cv2.imwrite(os.path.join(data_dir, 'vis', '%s_left.png' % (sample)), img_left)
        cv2.imwrite(os.path.join(data_dir, 'vis', '%s_right.png' % (sample)), img_right)
    return len(objects)

point_size, point_color, thickness = 1, (0, 0, 255), 4  # BGR
calib = Calibration()

if __name__ == "__main__":
    args = make_parser().parse_args()
    # 打开并加载JSON文件
    with open(args.json_path) as f:
        data = json.load(f)
        data_dir = data['rootDir']
        bmpFilename = data['bmpFilename']
        sample = bmpFilename[:bmpFilename.index("_")]
        cls_id = data['cls_id']
        length = float(data['length'])
        height = float(data['height'])
        width = float(data['width'])
        x_center = float(data['x_center'])
        y_bottom = float(data['y_bottom'])
        ry_ind = math.radians(int(data['ry_ind']))
        z_center = float(data['z_center'])
        left = data['left']
        top = data['top']
        right = data['right']
        bottom = data['bottom']
        if args.run_mode == 0:
            f_vis(data_dir, sample, height, width, length, x_center, y_bottom, z_center, ry_ind, left, top, right,
                  bottom, cls_id)
        elif args.run_mode == 1:
            f_write(data_dir, sample, height, width, length, x_center, y_bottom, z_center, ry_ind, left, top, right,
                    bottom, cls_id)
        elif args.run_mode == 2:
            num_obj = f_verify(data_dir, sample)
            # print('物体个数: %i' % num_obj)
            # 'w'参数表示写入模式，如果文件已存在，会先清空原有内容
            txt_path = os.path.join(data_dir, "temp", bmpFilename[:-4] + ".txt")
            with open(txt_path, 'w') as f:
                f.write('%i' % num_obj)
        elif args.run_mode == 3:  # 追加到模板
            pass


    # ===== parameter =====
    # data_dir = './'
    # sample = '00001'
    # cls_id = 0                             # *UI标识 " 0: 'Car', 1: 'Pedestrian' "
    # length = 4.6                           # ry = 0 时, x轴方向 (人的长度)
    # height = 1.522                          # ry = 0 时, y轴方向 (人的高度)
    # width = 1.748                           # ry = 0 时, z轴方向 (人的宽度)
    # x_center = -2.91                       # 正方向: 向右
    # y_bottom = 0.75                        # 正方向: 向下 (不是框的中心点, 而是框的底部点)
    # ry_ind = math.radians(0)                  # *UI标识 " 物体的正面方向 0: 朝镜头, 1: 朝左, 2: 朝右 "                  ~  90 / 179.99 / 0.01
    # z_center = 8.49
    # left, top, right, bottom = 193, 465, 500, 649

    # ===== 读图 (左图) =====
    # UI界面显示原图 img_left = np.array(Image.open(os.path.join(data_dir, 'images', '%s_left_ccd.bmp' % sample)))  # (960,1280,3), RGB

    # ===== 可视化 (左图) =====
    # f_vis(data_dir, sample, height, width, length, x_center, y_bottom, z_center, ry_ind, left, top, right, bottom, cls_id)
    # f_vis()用于计算框, 并输出保存标注后的图片
    # UI界面需要显示标注后的图 os.path.join(data_dir, 'vis', '%s_left.png' % (sample)

    # ====== 保存 ======
    # f_write(data_dir, sample, height, width, length, x_center, y_bottom, z_center, ry_ind, left, top, right, bottom, cls_id)
    # f_write()用于将当前物体的标注, 追加式保存至 .txt

    # ====== 验证 (左图) ======
    # num_obj = f_verify(data_dir, sample)
    # print('物体个数: %i' % num_obj)
    # f_verify()用于读取所有物体标注, 并输出保存标注后的图片
    # UI界面显示标注后的图 os.path.join(data_dir, 'vis', '%s_left.png' % (sample) + UI界面显示当前标注文件中含几个物体 num_obj