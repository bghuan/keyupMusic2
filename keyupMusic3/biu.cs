﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGestures.Core.Impl.Windows;
using static keyupMusic2.Common;

namespace keyupMusic3
{
    public class biu
    {
        public biu(Form parentForm)
        {
            huan = (Form2)parentForm;
        }
        public Form2 huan;
        bool listen_move = false;
        bool downing = false;
        bool downing2 = false;
        private Point start = Point.Empty;
        private int threshold = 10;
        bool handing = false;

        public void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (handing) return;
            handing = true;
            if (e.Msg != MouseMsg.WM_MOUSEMOVE) Task.Run(() => { yo(); });

            Task.Run(() => { ACPhoenix(e); });
            //Task.Run(() => { Douyin(e); });
            Douyin(e);
            Task.Run(() => { Devenv(e); });
            Task.Run(() => { ScreenEdgeClick(e); });
            Task.Run(() => { QQMusic(e); });
            Task.Run(() => { Other(e); });

            //TcpServer.socket_write(e.Msg.ToString());

            handing = false;
        }

        public void ACPhoenix(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (ProcessName != keyupMusic2.Common.ACPhoenix) return;

            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                listen_move = true;
                if (e.Y == 0 || e.Y == screenHeight - 1) { press(Keys.Space); }
                if (e.X == 0) { press(Keys.Tab); }
            }
            else if (e.Msg == MouseMsg.WM_LBUTTONUP)
            {
                listen_move = false;
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                downing2 = true;
                if (!(e.Y == 0 && (e.X <= screenWidth - 1 && e.X > screenWidth - 120)))
                    Task.Run(() =>
                    {
                        mouse_click2(50);
                        mouse_click2(50);
                        Thread.Sleep(50);
                        for (var i = 0; i < 50; i++)
                        {
                            if (!downing2) break;
                            mouse_click2(50);
                        }
                    });
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                downing2 = false;
                if ((e.Y < (493 * screenHeight / 1440) && e.Y > (190 * screenHeight / 1440)) && e.X < (2066 * screenWidth / 2560))
                    press(Keys.Space);
                if (try_press(1433, 1072, Color.FromArgb(245, 194, 55), () => { }))
                { }
                //退出观战
                if ((e.Y == 0 && (e.X <= screenWidth - 1 && e.X > screenWidth - 120)))
                {
                    //press("111"); press(Keys.F4); press("1625.1078");
                    if (is_alt()) return;
                    press("2478,51;2492,1299;1625.1078", 200);
                }
            }
            if (!listen_move) { return; }

            if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            {
                //var aaa = 1430 * 2160 / 1440;
                if (!listen_move || (e.Y < screenHeight - 10) || (e.X > screenWidth)) { return; }
                if (ProcessName2 == keyupMusic2.Common.ACPhoenix) { press(Keys.S, 0); ; }
                listen_move = false;
            }
        }
        public void Douyin(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (ProcessName != keyupMusic2.Common.douyin) return;
            //if (e.Msg != MouseMsg.WM_MOUSEMOVE) Task.Run(() => { huan.Invoke2(() => { huan.label1.Text = e.Msg.ToString(); }); });

            if (e.Msg == MouseMsg.WM_MOUSEMOVE && !downing) { return; }
            if (e.Msg == MouseMsg.WM_RBUTTONDOWN)
            {
                if (ProcessName2 != keyupMusic2.Common.douyin) return;
                if (e.X != 0) return;
                e.Handled = true;
                start = Position;
                downing = true;
            }
            else if (e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                if (ProcessName2 != keyupMusic2.Common.douyin) return;
                if (!downing) return;
                e.Handled = true;
                downing = false;
            }
            else if (e.Msg == MouseMsg.WM_MOUSEMOVE && downing == true)
            {
                int y = e.Y - start.Y;
                if (Math.Abs(y) > threshold)
                {
                    if (y > 0)
                        press(Keys.VolumeDown);
                    if (y < 0)
                        press(Keys.VolumeUp);
                    start = Position;
                }
                //Task.Run(() =>
                //{
                //    Thread.Sleep(11111);
                //    up_mouse();
                //});
            }
            //else if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            //{
            //    if ((e.X == 0) && e.Y == 0) { press(Keys.H); }
            //}
        }
        public void Devenv(MouseKeyboardHook.MouseHookEventArgs e)
        {
            //if (ProcessName != keyupMusic2.Common.devenv) return;

            //if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            //{
            //    if ((e.Y == 0) && (e.X < (2560 / 2)))
            //    {
            //        if (judge_color(82, 68, Color.FromArgb(189, 64, 77)))
            //            press([Keys.LControlKey, Keys.LShiftKey, Keys.F5]);
            //        else
            //            press([Keys.F5]);
            //    }
            //    else if ((e.Y == 0) && (e.X < 2560))
            //    {
            //        press([Keys.LShiftKey, Keys.F5]);
            //    }
            //}
        }
        bool left_left_click = false;
        bool left_down_click = false;
        bool right_up_click = false;
        private static List<MousePositionWithTime> recentMousePositions = new List<MousePositionWithTime>();

        public void ScreenEdgeClick(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            {
                if (e.X > (2560 * screenWidth / 2560) / 4 && left_left_click == false)
                    left_left_click = true;
                else if (e.Y < (1440 * screenHeight / 1440) / 4 * 3 && left_down_click == false)
                    left_down_click = true;
                else if (e.X < (2560 * screenWidth / 2560) / 1 && right_up_click == false)
                    right_up_click = true;
                if (Math.Abs(e.X - (1333 * screenWidth / 2560)) < 2 && e.Y == screenHeight - 1)
                    left_down_click = false;

                var not_allow = (ProcessName != keyupMusic2.Common.ACPhoenix) && (ProcessName != msedge);

                if (left_left_click && e.X == 0)
                {
                    if (!not_allow && IsFullScreen()) return;
                    if (ProcessName == douyin && IsFullScreen()) return;
                    left_left_click = false;
                    mouse_click2(0);
                }
                else if (left_down_click && e.Y == (screenHeight - 1) && e.X < screenWidth)
                {
                    if (!not_allow && IsFullScreen()) return;
                    left_down_click = false;
                    mouse_click2(0);
                }
                else if (right_up_click && e.Y == 0 && e.X > screenWidth)
                {
                    right_up_click = false;
                    mouse_click2(0);
                    if (e.X > screenWidth) press(Keys.F);
                }
            }
            else if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                left_left_click = false;
                left_down_click = false;
                right_up_click = false;
            }
        }

        private void QQMusic(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (ProcessName != keyupMusic2.Common.QQMusic) return;
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                ctrl_shift(true);
            }
        }
        public void Other(MouseKeyboardHook.MouseHookEventArgs e)
        {
            if (e.Msg == MouseMsg.WM_LBUTTONDOWN)
            {
                if (ProcessName == keyupMusic2.Common.msedge && (e.Y == (screenHeight - 1)))
                    press(Keys.PageDown, 0);
            }
            else if (e.Msg == MouseMsg.WM_LBUTTONUP)
            {
                if (e.X == 6719 || e.Y == 1619)
                {
                    HideProcess(keyupMusic2.Common.chrome); return;
                };
            }
            //else if (e.Msg == MouseMsg.WM_MOUSEMOVE)
            //{
            //    if (IsDrawingCircle(e.X, e.Y))
            //    {
            //        mouse_click(0);
            //        recentMousePositions = new List<MousePositionWithTime>();
            //    };
            //}
        }
        private static bool IsDrawingCircle(int x, int y)
        {
            // 假设圆心坐标为 (centerX, centerY)，半径为 radius
            int centerX = x;
            int centerY = y;
            int radius = 100;

            // 维护一个最近一段时间（比如 1 秒内）的鼠标位置列表
            DateTime now = DateTime.Now;
            recentMousePositions.RemoveAll(pos => (now - pos.When).TotalSeconds > 1);
            recentMousePositions.Add(new MousePositionWithTime { X = x, Y = y, When = now });
            int totalCount = recentMousePositions.Count;

            if (totalCount < 33) return false;

            // 判断最近一段时间内的位置是否大致形成一个圆
            int countInCircle = recentMousePositions.Count(pos =>
            {
                var a = (pos.X - centerX) * (pos.X - centerX) + (pos.Y - centerY) * (pos.Y - centerY);
                return a >= 52 * 52 && a <= radius * radius;
            });
            double ratio = (double)countInCircle / totalCount;

            // 根据圆的方程判断点是否在圆上或圆内
            bool inCircle = (x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= radius * radius;

            return inCircle && ratio > 0.5; // 可以根据实际情况调整这个比例阈值
        }

        // 辅助结构体存储鼠标位置和时间
        private struct MousePositionWithTime
        {
            public int X;
            public int Y;
            public DateTime When;
        }
    }
}
