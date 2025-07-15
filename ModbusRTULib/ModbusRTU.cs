using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusRTULib
{
    public class ModbusRTU
    {
        #region 构造方法

        public ModbusRTU()
        {
            // 实例化串口通信对象
            serialPort = new SerialPort();

            // TODO
        }

        #endregion

        #region 字段或属性

        /// <summary>
        /// 串口通信对象
        /// </summary>
        SerialPort serialPort = null;

        /// <summary>
        /// 串口读取超时时间，单位：ms
        /// </summary>
        public int ReadTimeout { get; set; } = 2000;

        /// <summary>
        /// 串口写入超时时间，单位：ms
        /// </summary>
        public int WriteTimeout { get; set; } = 2000;


        private bool dtrEnable = false;
        /// <summary>
        /// DTR使能标志，如果不用DTR，对方可能压根不在线。
        /// </summary>
        public bool DtrEnable 
        {
            get { return dtrEnable; }
            set { dtrEnable = value; this.serialPort.DtrEnable = dtrEnable; } 
        }

        private bool rtsEnable = false;
        /// <summary>
        /// TRS使能标志，如果不用RTS直接狂发消息，对方可能忙死漏看；
        /// </summary>
        public bool RrtsEnable
        {
            get { return rtsEnable; }
            set { rtsEnable = value; this.serialPort.RtsEnable = rtsEnable; }
        }






        #endregion

        #region 方法

        #region 建立连接和断开连接

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="baudRate">波特率，缺省值9600</param>
        /// <param name="parity">校验位，缺省值Parity.None</param>
        /// <param name="dataBits">数据位，缺省值8</param>
        /// <param name="stopBits">停止位，缺省值StopBits.One</param>
        /// <returns>返回布尔值，表示连接是否成功</returns>
        public bool Connect(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            // 如果当前串口处于已连接状态，先关闭一下
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }

            // 串口属性设置
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.Parity = parity;
            serialPort.DataBits = dataBits;
            serialPort.StopBits = stopBits;
            serialPort.ReadTimeout = this.ReadTimeout;
            serialPort.WriteTimeout = this.WriteTimeout;


            try
            {
                // 如果 串口被占用 or 串口不存在 时，调用 Open 会抛异常，需要 try-catch 捕获
                serialPort.Open();
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

            return true;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        #endregion

        #region 01H读取输出线圈

        //*************************************************************
        // 发送报文格式：从站地址 + 功能码 + 开始线圈地址 + 线圈数量 + CRC
        // 接受报文格式：从站地址 + 功能码 + 字节计数 + 数据 + CRC
        //*************************************************************

        /// <summary>
        /// 读取输出线圈
        /// </summary>
        /// <param name="slaveId">从站地址</param>
        /// <param name="start">开始线圈地址</param>
        /// <param name="lenght">线圈数量</param>
        /// <returns>返回读取到的数据</returns>
        public byte[] ReadOutputCoils(byte slaveId, short start, ushort lenght)
        {
            List<byte> sendCommand = new List<byte>();

            // 第一步：拼接报文
            // - 拼接从站地址(1)
            sendCommand.Add(slaveId);
            // - 拼接功能码(1)
            sendCommand.Add(0x01);
            // - 拼接开始线圈地址(2, 高位在前)
            sendCommand.Add((byte)(start / 256));
            sendCommand.Add((byte)(start % 256));
            // - 拼接线圈数量(2, 高位在前)
            sendCommand.Add((byte)(lenght / 256));
            sendCommand.Add((byte)(lenght % 256));
            // - 拼接CRC(2, 低位在前)


            // 第二步：发送报文

            // 第三步：接收报文

            // 第四步：验证报文

            // 第五步：解析报文

        }

        #endregion

        #region 02H读取输入线圈



        #endregion

        #region 03H读取输出寄存器



        #endregion

        #region 04H读取输入寄存器



        #endregion

        #region 05H预置单线圈



        #endregion

        #region 06H预置单寄存器



        #endregion

        #region 0FH预置多线圈



        #endregion

        #region 10H预置多寄存器



        #endregion

        #region CRC校验



        #endregion

        #endregion
    }
}
