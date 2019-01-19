using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace RemoteMouseServer
{
    class UDPServerAssinc
    {
        //Serviço UDP 
        static UdpClient U;
        public static void ServerStart()
        {            
            //Port de escuta do protocolo UDP
            int port = 27051;
            //Criação do serviço UDP
            U = new UdpClient(port);

            //Faz o primeiro pedido de leitura (os próximos serão feitos na callback, a cada mensagem que chegar)
            //Note que o o método BeginReceive é assincrono, isto é, ele não bloqueia o programa. 
            U.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            //A partir de agora, o programa pode ficar parado ou fazendo qualquer coisa.
            //Toda vez que chegar uma mensagem, o processamento será interrompido para execução da ReceiveCallback onde a mensagem
            //será lida e processada.

        }
        //-----------------------------------------------------------------
        //Esta rotina trata as mensagens UDP, em um thread separado, conforme elas vão chegando.
        //

        private static void ReceiveCallback(IAsyncResult res)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] received = U.EndReceive(res, ref RemoteIpEndPoint);
            Console.WriteLine(Encoding.ASCII.GetString(received));
            if(Encoding.ASCII.GetString(received).Contains("M;"))
            RemoteMouse.Position = Encoding.ASCII.GetString(received);
            else 
            RemoteKeybd.KeybdButton = Encoding.ASCII.GetString(received);
            //faz outro pedido de leitura antes de devolver o 
            U.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
    }
}
