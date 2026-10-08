# -*- coding: utf-8 -*-
"""阶段3：把 Common.cs 按主题拆分为 partial 文件（Common.Screen/Window/System/Util）。"""
import io, os

ROOT = r"C:\Users\bu\source\repos\keyupMusic2\Core"
SRC = os.path.join(ROOT, "Common.cs")

with open(SRC, "rb") as f:
    raw = f.read()
has_bom = raw.startswith(b"\xef\xbb\xbf")
lines = raw.decode("utf-8-sig" if has_bom else "utf-8").splitlines(keepends=True)

# 头部（using + namespace 行）: 1..16（namespace keyupMusic4 { 在第16行"namespace keyupMusic4"后是第17行"{"）
# 文件第1-15行为 using/类型别名，第16行 namespace keyupMusic4，第17行 "{"，第18行起为 partial class Common
# 定位:
for i, l in enumerate(lines, start=1):
    if l.startswith("namespace"):
        ns_line = i - 1  # 0-based
        break
# ns_line 是 namespace 行(0-based)，其下一行是 "{"（class 内），再下一行是 partial class Common
# 实际文件:
# 15 namespace keyupMusic4
# 16 {
# 17     public partial class Common
# 18     {
# 19         ...字段/方法
# 尾部: "    }\n" + "}\n"

# 提取头部: 行1..(ns_line) 即 using 部分（不含 namespace 行）; namespace 行 + { + partial class 行 + { 由新文件重建
using_head = lines[:ns_line]
body_all = lines[ns_line:]  # 从 namespace 行到结尾（含闭合）

# 找到 partial class Common 行的位置（在 body_all 中）
class_idx = None
for i, l in enumerate(body_all):
    if "partial class Common" in l:
        class_idx = i
        break
# body 内容（真正的方法/字段）: class_idx+2 起（跳过 "    {"），到 len(body_all)-2（排除 class 闭合 "    }" 与 namespace 闭合 "}"）
content_lines = body_all[class_idx+2 : len(body_all)-2]

# 区间定义: (start, end) 1-based 全文件行号 -> 目标文件
# 行号基于原始 lines
SECTIONS = {
    "Common.Screen.cs": [
        (82, 100), (101, 115), (144, 175), (176, 179), (180, 197), (198, 206),
        (207, 241), (288, 344), (486, 558), (1476, 1526),
    ],
    "Common.Window.cs": [
        (430, 485), (646, 966), (1125, 1186), (1248, 1259), (1292, 1346), (1527, len(lines) - 2),
    ],
    "Common.System.cs": [
        (345, 383), (420, 425), (967, 996), (1239, 1247), (1260, 1290),
    ],
    "Common.Util.cs": [
        (116, 143), (242, 286), (384, 419), (426, 429), (559, 645), (997, 1124), (1378, 1475),
    ],
}

# 校验覆盖：标记被抽取的行
taken = set()
for _, secs in SECTIONS.items():
    for s, e in secs:
        for n in range(s, e + 1):
            assert n not in taken, "区间重叠 at line %d" % n
            taken.add(n)

# 保留行 = 未抽取的行（从 content 区中）
content_keep = []
content_taken = set()
for idx0 in range(len(content_lines)):
    lineno = class_idx + 1 + idx0 + 1  # 1-based 全文件行号 = ns_line + 1 + class_idx + 1 + idx0... 简化: 直接由 lines 索引
# 更简单：用行号直接判断。content_lines[i] 对应全文件行号 base + i
base = ns_line + 1 + class_idx + 2  # 第1行(1-based) => index 0; content_lines[0] 是全文件第 (ns_line+class_idx+3) 行(1-based)
# 重新推导: body_all 从全文件第 (ns_line+1) 行(1-based) 开始。class_idx 是 body_all 内索引，故 partial class 行是全文件第 (ns_line+1+class_idx) 行。
# content_lines[0] = 全文件第 (ns_line+1+class_idx+2) 行（class 行后一行是 "    {"，其后再一行才是内容）
cstart = ns_line + 1 + class_idx + 2  # 1-based

keep_blocks = []
cur = []
for idx0, l in enumerate(content_lines):
    lineno = cstart + idx0
    if lineno in taken:
        if cur:
            keep_blocks.append("".join(cur))
            cur = []
    else:
        cur.append(l)
if cur:
    keep_blocks.append("".join(cur))

# 生成新 partial 文件
ns_decl = lines[ns_line] + lines[ns_line+1]  # "namespace keyupMusic4\n{\n"
class_decl = lines[ns_line+2]  # "    public partial class Common\n"
# 用 class_decl 即可（partial class Common）——但我们要生成多个文件，每个都声明 partial class Common
indent = "    "

for fname, secs in SECTIONS.items():
    parts = []
    for s, e in secs:
        parts.append("".join(lines[s-1:e]))
    body = "\n".join(parts)  # 可能缺空行，无碍
    # 构造新文件: using 头 + namespace { partial class Common { body } }
    new_content = "".join(using_head) + ns_decl + class_decl + "    {\n" + body + "    }\n}\n"
    # body 每行已有缩进（4 空格），直接插入
    outp = os.path.join(ROOT, fname)
    with open(outp, "wb") as f:
        f.write(new_content.encode("utf-8-sig"))
    print("created %s (%d 行)" % (fname, len(parts)))

# 重写 Common.cs：using 头 + namespace { partial class Common { 保留块 } }
keep_body = "\n".join(keep_blocks)
if not keep_body.endswith("\n"):
    keep_body += "\n"
new_main = "".join(using_head) + ns_decl + class_decl + "    {\n" + keep_body + "    }\n}\n"
with open(SRC, "wb") as f:
    f.write(new_main.encode("utf-8-sig"))
print("Common.cs 重写完成，保留 %d 块" % len(keep_blocks))
