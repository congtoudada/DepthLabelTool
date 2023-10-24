import argparse
import os
import traceback
import numpy
from PIL import Image
import json

# 路径
# path= '/'
# 编号，从0开始
# number = 0
# 第一个区域的左上角点的横(宽)纵(高)坐标, 第二个区域的左上角点的横纵坐标
# left_top = [(x11, y11), (x21, y21)]
# 第一个区域的右下角点的横纵坐标, 第二个区域的右下角点的横纵坐标
# right_bottom = [(x12, y12), (x21, y21)]
# 第一个区域的测量深度, 第二个区域的测量深度
# depth = [d1, d2]

# # 例子
# path = './Resources/00001_left_ccd.bmp'
# number = 0
# left_top = [(200, 200), (600, 600)]
# right_bottom = [(500, 500), (700, 700)]
# depth = [200, 300]
#
# h, w = 960, 1280 # 可见光图像的尺寸
#
# depthmap = numpy.zeros((h, w))
# for idx, (lt, rb, d) in enumerate(zip(left_top, right_bottom, depth)):
#     x1, y1 = lt
#     x2, y2 = rb
#     depthmap[y1:y2, x1:x2] = d
#
# depthmap = (depthmap * 256.0).astype('uint16')
# depth_buffer = depthmap.tobytes()
# depthsave = Image.new("I", depthmap.T.shape)
# depthsave.frombytes(depth_buffer, 'raw', "I;16")
# depthsave.save(path+'{:0>5d}.png'.format(number))


def make_parser():
    parser = argparse.ArgumentParser("Export Depth PNG!")
    parser.add_argument("--json_path", type=str, default=None)
    parser.add_argument("--img_dir", type=str, default=None)
    # 通常 export_dir和label_dir是一起的
    parser.add_argument("--export_dir", type=str, default=None)
    parser.add_argument("--label_dir", type=str, default=None)
    return parser

def export(img_path, export_path, width, height, left_top, right_bottom, depth):
    depthmap = numpy.zeros((height, width))
    for idx, (lt, rb, d) in enumerate(zip(left_top, right_bottom, depth)):
        x1, y1 = lt
        x2, y2 = rb
        depthmap[y1:y2, x1:x2] = d

    depthmap = (depthmap * 256.0).astype('uint16')
    depth_buffer = depthmap.tobytes()
    depthsave = Image.new("I", depthmap.T.shape)
    depthsave.frombytes(depth_buffer, 'raw', "I;16")
    depthsave.save(export_path)

if __name__ == "__main__":
    try:
        args = make_parser().parse_args()
        # 打开并加载JSON文件
        with open(args.json_path) as f:
            data = json.load(f)
            export_filename: str = data['bmpFilename']
            export_filename = export_filename.replace("left", "depth").replace("bmp", "png")
            export_path = os.path.join(args.export_dir, export_filename)
            # img_path = os.path.join(args.img_dir, data['bmpFilename'])
            type_info_list = data['typeInfoList']
            left_top = []
            right_bottom = []
            depth = []
            for item in data['resultList']:
                left_top.append((item['lt_x'], item['lt_y']))
                right_bottom.append((item['rb_x'], item['rb_y']))
                depth.append(float(type_info_list[item['typeIdx']]))

            export(None, export_path,
                   data['img_width'], data['img_height'], left_top, right_bottom, depth)


        # 输出加载的JSON数据
        # print(data)
        # print(args.json_path)
    except:
        print(traceback.format_exc())

