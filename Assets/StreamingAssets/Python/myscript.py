import argparse


def make_parser():
    parser = argparse.ArgumentParser("Unity Demo!")
    parser.add_argument("--expn", type=str, default="Unity")
    return parser

if __name__ == '__main__':
    args = make_parser().parse_args()
    print(args.expn)

