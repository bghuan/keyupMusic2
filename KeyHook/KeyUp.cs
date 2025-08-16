using static keyupMusic2.Common;
using static keyupMusic2.KeyboardMouseHook;

namespace keyupMusic2
{
    public class KeyUp
    {
        public static bool numkeyed = false;
        public static List<Keys> numkeylist = new List<Keys> { };
        public static void quick_number2(Keys key)
        {
            var left = key.Equals(Keys.Left);
            var down = key.Equals(Keys.Down);
            var right = key.Equals(Keys.Right);
            var _left = !left && is_down_vir(Keys.Left);
            var _down = !down && is_down_vir(Keys.Down);
            var _right = !right && is_down_vir(Keys.Right);
            var _all = (_left ? 1 : 0) + (_down ? 1 : 0) + (_right ? 1 : 0) == 2;
            var count = (_left ? 1 : 0) + (_down ? 1 : 0) + (_right ? 1 : 0);
            var downkey = count == 1 ? (_left ? Left : (_down ? Down : (_right ? Right : None))) : Keys.None;

            if (count == 0)
            {
                if (!numkeylist.Contains(key))
                {
                    if (left) press(D1);
                    if (down) press(D2);
                    if (right) press(D3);
                }
                else
                {
                    numkeylist = new List<Keys> { };
                }
            }
            else if (count == 1)
            {
                if (!numkeylist.Contains(key))
                {
                    if (_left)
                    {
                        if (down) press(D4);
                        if (right) press(D5);
                    }
                    else if (_down)
                    {
                        if (left) press(D6);
                        if (right) press(D7);
                    }
                    else if (_right)
                    {
                        if (left) press(D8);
                        if (down) press(D9);
                    }
                    if (!numkeylist.Contains(downkey))
                        numkeylist.Add(downkey);
                }
                else
                {
                    numkeylist = new List<Keys> { downkey };
                }
            }
            else if (count == 2)
            {
                press(D0);
                numkeylist = new List<Keys> { Left, Down, Right };
            }

            return;
            if (_all)
            {
                press(D0);
                numkeylist = new List<Keys> { Left, Down, Right };
            }
            else if (!_left && !_down && !_right)
            {
                //if (numkeylist.Count == 0)
                if (!numkeylist.Contains(key))
                {
                    if (left && !down && !right) press(D1);
                    if (!left && down && !right) press(D2);
                    if (!left && !down && right) press(D3);
                }
                else
                {
                    numkeylist = new List<Keys> { };
                }
            }
            else if (_left && !_down && !_right)
            {
                if (!numkeylist.Contains(key))
                {
                    if (!left && down && !right) press(D4);
                    if (!left && !down && right) press(D5);
                    if (!numkeylist.Contains(Keys.Left))
                        numkeylist.Add(Keys.Left);
                }
                else
                {
                    numkeylist = new List<Keys> { Left };
                }
            }
            else if (!_left && _down && !_right)
            {
                if (!numkeylist.Contains(key))
                {
                    if (left && !down && !right) press(D6);
                    if (!left && !down && right) press(D7);
                    if (!numkeylist.Contains(Keys.Down))
                        numkeylist.Add(Keys.Down);
                }
                else
                {
                    numkeylist = new List<Keys> { Down };
                }
            }
            else if (!_left && !_down && _right)
            {
                if (!numkeylist.Contains(key))
                {
                    if (left && !down && !right) press(D8);
                    if (!left && down && !right) press(D9);
                    if (!numkeylist.Contains(Keys.Right))
                        numkeylist.Add(Keys.Right);
                }
                else
                {
                    numkeylist = new List<Keys> { Right };
                }
            }
        }
        public static bool yo(KeyboardMouseHook.KeyEventArgs e)
        {
            if (e.Type == KeyType.Down) return false;
            if (Position.Y == 0)
                switch (e.key)
                {
                    case Keys.Left:
                    case Keys.Down:
                    case Keys.Right:
                        quick_number2(e.key); break;
                }

            switch (e.key)
            {
                case Keys.Home:
                case Keys.End:
                    try { bmpScreenshot.Dispose(); } catch (NullReferenceException eee) { }
                    break;
            }

            if (LongPressKey == e.key) LongPressKey = Keys.None;
            return true;
        }
    }
}
