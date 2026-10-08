# -*- coding: utf-8 -*-
"""阶段1（补）：命名空间统一 keyupMusic2 -> keyupMusic4，兼容 GBK 编码文件，全部转 UTF-8 with BOM。"""
import os

ROOT = r"C:\Users\bu\source\repos\keyupMusic2"
SKIP_DIRS = {"bin", "obj", ".git", "node_modules"}

def decode(raw):
    if raw.startswith(b"\xef\xbb\xbf"):
        return raw.decode("utf-8-sig")
    try:
        return raw.decode("utf-8")  # 严格模式
    except UnicodeDecodeError:
        return raw.decode("gb18030")  # GBK/ANSI 全字符集

def process_file(path):
    raw = open(path, "rb").read()
    try:
        text = decode(raw)
    except Exception as e:
        return ("FAIL", str(e))
    orig = text
    text = text.replace("Common.keyupMusic2", "Common.AppName")
    text = text.replace("public const string keyupMusic2 =", "public const string AppName =")
    text = text.replace("keyupMusic2", "keyupMusic4")
    if text == orig:
        return ("SKIP", None)
    open(path, "wb").write(text.encode("utf-8-sig"))  # 统一 UTF-8 with BOM
    return ("CHANGED", None)

changed, skipped, failed = [], [], []
for dirpath, dirnames, filenames in os.walk(ROOT):
    dirnames[:] = [d for d in dirnames if d not in SKIP_DIRS]
    for fn in filenames:
        if fn.endswith(".cs"):
            p = os.path.join(dirpath, fn)
            st, err = process_file(p)
            rel = os.path.relpath(p, ROOT)
            if st == "CHANGED":
                changed.append(rel)
            elif st == "FAIL":
                failed.append((rel, err))
            else:
                skipped.append(rel)

print("changed: %d" % len(changed))
for c in changed:
    print("  " + c)
print("skipped(no keyupMusic2): %d" % len(skipped))
print("failed: %d" % len(failed))
for f, e in failed:
    print("  FAIL %s: %s" % (f, e))
