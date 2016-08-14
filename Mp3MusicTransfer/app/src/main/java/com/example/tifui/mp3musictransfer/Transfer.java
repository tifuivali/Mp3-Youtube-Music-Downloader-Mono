package com.example.tifui.mp3musictransfer;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.Arrays;
import java.util.EventListener;

/**
 * Created by tifui on 09.02.2016.
 */
public class Transfer {

    String dstAdress;
    int dstPort;

    Transfer(String Adress, int Port) {
        dstAdress = Adress;
        dstPort = Port;
    }

    public void SetIpAdress(String ipAdress)
    {
        dstAdress=ipAdress;
    }
    public String GetIpAdress()
    {
        return  dstAdress;
    }
    public void SetPort(int port)
    {
        dstPort=port;
    }
    public int GetPort()
    {
        return dstPort;
    }
    public Socket Connect()
    {
        try {
            return new Socket(dstAdress, dstPort);
        }
       catch (UnknownHostException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        finally {
            return null;
        }
    }
    public EventListener OnProgressChenge;

    //recive
    public void ReciveFile()
    {



    }


}
