﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace EarthLiveSharp
{
    public partial class mainForm : Form
    {
        bool serviceRunning = false;
        MenuItem startService = new MenuItem("Start Service");
        MenuItem stopService = new MenuItem("Stop Service");
        MenuItem settingsMenu = new MenuItem("Settings");
        MenuItem quitService = new MenuItem("Quit");
        ContextMenu trayMenu = new ContextMenu();

        public mainForm()
        {
            InitializeComponent();
            createContextMenu();
            notifyIcon1.ContextMenu = trayMenu;
        }
        private void createContextMenu()
        {
            this.trayMenu.MenuItems.Add(startService);
            this.trayMenu.MenuItems.Add(stopService);
            this.trayMenu.MenuItems.Add(settingsMenu);
            this.trayMenu.MenuItems.Add(quitService);
            startService.Click += new EventHandler(this.startService_Click);
            stopService.Click += new EventHandler(this.stopService_Click);
            settingsMenu.Click += new EventHandler(this.settingsMenu_Click);
            quitService.Click += new EventHandler(this.quitService_Click);

            timer3.Interval = 1000;
            timer3.Start();
            contextMenuSetter();
        }

        private void startService_Click(object sender, EventArgs e)
        {
            startLogic();
        }
        private void stopService_Click(object sender, EventArgs e)
        {
            stopLogic();
        }
        private void settingsMenu_Click(object sender, EventArgs e)
        {
            settingsForm f1 = new settingsForm();
            f1.ShowDialog();
        }
        private void quitService_Click(object sender, EventArgs e)
        {
            var confirmIfQuitting = MessageBox.Show("Are you sure you want to quit?","Stopping Service", MessageBoxButtons.YesNo);
            if (confirmIfQuitting == DialogResult.Yes)
            {
                stopLogic();
                Application.Exit();
            }

        }


        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://himawari8.nict.go.jp/");
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            settingsForm f1 = new settingsForm();
            f1.ShowDialog();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            startLogic();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            stopLogic();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            scraper.size = Cfg.size;
            scraper.zoom = Cfg.zoom;
            scraper.image_folder = Cfg.image_folder;
            scraper.image_source = Cfg.image_source;
            scraper.UpdateImage();
            notifyIcon1.ShowBalloonTip(1000, "地球照片已更新", scraper.last_imageID, ToolTipIcon.Info);
            scraper.AddPicture();
            Wallpaper.Set(scraper.image_folder+"\\wallpaper.bmp");
        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
                if (!Cfg.autostart)
                {
                    notifyIcon1.ShowBalloonTip(1000, "", "EarthLive# is running", ToolTipIcon.Warning);
                }
            }
        }   

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            button_stop.Enabled = false;
            if (Cfg.autostart)
            {
                button_start.PerformClick();
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        //All logic pertaining to stopping the service
        private void stopLogic()
        {
            if (serviceRunning)
            {
                timer1.Stop();
                timer2.Stop();
                button_start.Enabled = true;
                button_stop.Enabled = false;
                button_settings.Enabled = true;
                runningLabel.Text = "Not Running";
                runningLabel.ForeColor = Color.DarkRed;
                serviceRunning = false;
            }
            else if (!serviceRunning) MessageBox.Show("Service is not currently running");
            contextMenuSetter();
        }

        //All logic pertaining to starting the service
        private void startLogic()
        {
            scraper.size = Cfg.size;
            scraper.zoom = Cfg.zoom;
            scraper.image_folder = Cfg.image_folder;
            scraper.image_source = Cfg.image_source;
            scraper.last_imageID = "0"; // reset the scraper record.
            if (!serviceRunning)
            {
                button_start.Enabled = false;
                button_stop.Enabled = true;
                button_settings.Enabled = false;
                scraper.UpdateImage();
                scraper.AddPicture();
                timer1.Interval = Cfg.interval * 1000 * 60;
                timer1.Start();

                
                Wallpaper.SetDefaultStyle();
                Wallpaper.Set(scraper.image_folder + "\\wallpaper.bmp");
                serviceRunning = true;
                runningLabel.Text = "    Running";
                runningLabel.ForeColor = Color.DarkGreen;
            }
            else
            {
                MessageBox.Show("Service already running");
            }
            contextMenuSetter();
        }

        //checks if service running and changes context menu based on result.
        private void contextMenuSetter()
        {
            if (serviceRunning)
            {
                startService.Enabled = false;
                stopService.Enabled = true;
            }

            if (!serviceRunning)
            {
                stopService.Enabled = false;
                startService.Enabled = true;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {

            //Program.Trace.WriteLine("[get latest ImageID] " + imageID);
            
            scraper.AddPicture();
            label3.Text = "壁纸时钟：" + DateTime.Now.ToString("HH:mm:ss");
            //notifyIcon1.ShowBalloonTip(1000, "壁纸时钟已更新", scraper.last_imageID, ToolTipIcon.Info);
            Wallpaper.Set(scraper.image_folder + "\\wallpaper.bmp");
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            label4.Text = "系统时间:" + DateTime.Now.ToString("HH:mm:ss");
            if (DateTime.Now.Second.Equals(00))
            {
                //timer3.Stop();
                //notifyIcon1.ShowBalloonTip(1000, "timer2 start",DateTime.Now.Second.ToString(), ToolTipIcon.Info);
                timer2.Interval = 1000 * 60;
                timer2.Start();
            }


        }


    }
}
