﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPChatWF
{
    public partial class Form1 : Form
    {
        int localPort, remotePort;
        string remoteAddress;
        string local = "127.0.0.1";


        public Form1()
        {
            InitializeComponent();
            btnconnect.Enabled = false;
            btnsend.Enabled = false;
            tbrecPort.MaxLength = 4;
            tbsendport.MaxLength = 4;
            labelConn.Visible = false;
            
        }

        private void SendMessage()
        {
            UdpClient sender = new UdpClient();

            try
            {
                
                    string message = txbmessage.Text;

                    if (listBox1.InvokeRequired&& message!="")
                    {
                        //listBox1.Invoke(new Action(() => listBox1.Items.Add("Введите сообщение")));
                        listBox1.Invoke(new Action(() => listBox1.Items.Add("(You): "+message)));
                    }

                    Thread.Sleep(1000);

                    if (txbmessage.Text != "")
                    {
                        byte[] data = Encoding.UTF8.GetBytes("("+tbname.Text + "): " + message);

                        sender.Send(data, data.Length, remoteAddress, remotePort);
                        txbmessage.Text = "";
                    }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        private void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort);
            IPEndPoint iPEndPoint = null;

            try
            {
                while (true)
                {

                    byte[] bytes = receiver.Receive(ref iPEndPoint);

                    string message = Encoding.UTF8.GetString(bytes);
                    Thread.Sleep(1000);

                    if (message!="")
                       listBox1.Invoke(new Action(() => listBox1.Items.Add($"Входящее сообщение: {message} ")));

                    //listBox1.Items.Add($"Входящее сообщение: {message} ");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                receiver.Close();
            }

        }

        private void btnconnect_Click(object sender, EventArgs e)
        {

            try
            {
                localPort = Int32.Parse(tbrecPort.Text); //порт входящ
                remotePort = Int32.Parse(tbsendport.Text); //порт исход

                if (radYourIp.Checked == true)
                    remoteAddress=tbip.Text;    //IP
                if(radLocalIp.Checked == true)
                {
                    remoteAddress = local;
                }

                Task receivTask = new Task(ReceiveMessage);
                receivTask.Start();

                labelConn.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbname_TextChanged(object sender, EventArgs e)
        {
            if(tbname.Text!=""&& tbsendport.Text!="" && tbrecPort.Text!="")
            {
                btnconnect.Enabled = true;
                btnsend.Enabled = true;
            }
        }

        private void tbip_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbsendport_TextChanged(object sender, EventArgs e)
        {
            if (tbname.Text != "" && tbsendport.Text != "" && tbrecPort.Text != "")
            {
                btnconnect.Enabled = true;
                btnsend.Enabled = true;
            }
        }

        private void btnsend_Click(object sender, EventArgs e)
        {

            //SendMessage();
            //ReceiveMessage();
            Task sendTask = new Task(SendMessage);
            sendTask.Start();
        }

        private void tbrecPort_TextChanged(object sender, EventArgs e)
        {
            if (tbname.Text != "" && tbsendport.Text != "" && tbrecPort.Text != "")
            {
                btnconnect.Enabled = true;
                btnsend.Enabled = true;
            }
        }
    }
}
