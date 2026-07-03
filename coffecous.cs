using System;
using System.IO.Ports;

public class CommBridge
{
    private SerialPort _serialPort;
    
    // O evento que o usuário final vai "pendurar" a lógica
    public event EventHandler<byte[]> OnDataReceived;

    // Enviar dados para COM
    public void DCOM(int portNumber, byte data)
    {
        string portName = "COM" + portNumber;
        using (_serialPort = new SerialPort(portName, 9600))
        {
            _serialPort.Open();
            _serialPort.Write(new byte[] { data }, 0, 1);
            _serialPort.Close();
        }
    }

    // Método para escuta (o 'U' que você mencionou)
    public void U_Listen(string portName)
    {
        _serialPort = new SerialPort(portName, 9600);
        _serialPort.DataReceived += (s, e) => {
            byte[] buffer = new byte[1];
            _serialPort.Read(buffer, 0, 1);
            
            // Dispara o evento definido pelo usuário
            if (OnDataReceived != null) 
                OnDataReceived(this, buffer);
        };
        _serialPort.Open();
    }
}