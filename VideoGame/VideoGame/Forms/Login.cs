﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Sandbox.Classes;
using VideoGame.Forms;
using Settings = VideoGame.Classes.Settings;

namespace Sandbox.Forms {
    public partial class Login : Form {
        public static int PlayerId;
        public static string UserName;
        public string Password;
        public Login() {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e) {
            DatabaseConnector.SetConnectionString(Settings.ServerName, Settings.Username, Settings.Password, Settings.DatabaseName);
            //Login, see if user exists, if not ask for register
            PlayerId = DatabaseConnector.GetPlayerId(tbUser.Text);
            UserName = tbUser.Text;
            Password = tbPassword.Text;
            if (PlayerId != 0) {
                //Player has been found
                if (DatabaseConnector.CheckPassword(UserName, Password)) {
                    Launcher.isIngelogd = true;
                    this.Close();
                }
                else { MessageBox.Show("Password or username is incorrect"); }
            }
            else {
                //Playername does not exists
                var mbAnswer = MessageBox.Show("Player name not found, would you like to register it?", "Register", MessageBoxButtons.YesNo);
                switch (mbAnswer) {
                case DialogResult.Yes:
                    if (string.IsNullOrEmpty(tbPassword.Text)) {
                        MessageBox.Show("Please enter a password and try again");
                    }
                    else {
                        DatabaseConnector.AddCharacter(tbUser.Text, tbPassword.Text);
                        MessageBox.Show($"Succesfully added {tbUser.Text} to database");
                        Launcher.isIngelogd = true;
                        this.Close();
                    }
                    break;
                case DialogResult.No: break;
                }
            }
        }
    }
}
