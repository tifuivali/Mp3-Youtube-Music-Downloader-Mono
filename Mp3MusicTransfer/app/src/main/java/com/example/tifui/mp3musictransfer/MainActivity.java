package com.example.tifui.mp3musictransfer;

import android.app.AlertDialog;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.bluetooth.BluetoothAdapter;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.renderscript.Int3;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.NotificationCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.util.Xml;
import android.view.MenuInflater;
import android.view.View;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import org.w3c.dom.Text;

import java.io.BufferedInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInput;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.math.BigInteger;
import java.net.Socket;
import java.net.UnknownHostException;
import java.nio.ByteBuffer;
import java.sql.Array;
import java.util.Arrays;

import javax.net.ssl.SSLSocket;

public class MainActivity extends AppCompatActivity {
    EditText editText_status;
    EditText  editText_Port;
    EditText editText_Ip;
    ProgressBar progressBar_transferProgress;
    Button button_start;
    Button button_stop;
    Transfer transfer;
    TextView textView_connected;
    TextView textView_disconnected;
    TextView textView_currentTask;
    boolean isTransfering=false;
    int IdNotdification=0;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        MenuItem item_open_downloads =(MenuItem)findViewById(R.id.action_open_Transfers);



       /*    set floating button
        final FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
            }
        });
        */

        editText_Ip=(EditText)findViewById(R.id.editTex_IP);
        editText_Port=(EditText)findViewById(R.id.editText_port);
        editText_status=(EditText)findViewById(R.id.editText_status);
        button_stop=(Button)findViewById(R.id.button_stop);
        button_stop.setEnabled(false);
        textView_connected=(TextView)findViewById(R.id.textView_Connected);
        textView_disconnected=(TextView)findViewById(R.id.textViewDisconected);
        textView_currentTask=(TextView)findViewById(R.id.textView_curentTask);
        progressBar_transferProgress=(ProgressBar)findViewById(R.id.progressBar);
        button_stop.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                if (!isTransfering) {
                    Toast.makeText(MainActivity.this, "No conection started!", Toast.LENGTH_SHORT).show();
                    return;
                }
                progressBar_transferProgress.setProgress(0);
                editText_status.append("Canceled!\r\n");
                transfer.cancel(true);
                isTransfering = false;


            }
        });

        button_start=(Button)findViewById(R.id.button_start);
        button_start.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                if(isTransfering)
                {
                    Toast.makeText(MainActivity.this, "An trnsfer already running...!", Toast.LENGTH_SHORT).show();
                    return;
                }
                if(editText_Ip.getText().length()==0||editText_Port.getText().length()==0)
                {Toast.makeText(MainActivity.this, "Set an ip and port!", Toast.LENGTH_SHORT).show();
                    return;
                }
                try {
                    transfer = new Transfer(editText_Ip.getText().toString(), Integer.parseInt(editText_Port.getText().toString()));
                    transfer.execute();
                    button_stop.setEnabled(true);
                }
                catch (Exception exc)
                {
                    editText_status.append("Eror:"+exc.getMessage()+"\r\n");
                }

            }
        });
    }



    public String getPhoneName() {
        BluetoothAdapter myDevice = BluetoothAdapter.getDefaultAdapter();
        String deviceName = myDevice.getName();
        return deviceName;
    }


    private void Notify(String notificationTitle, String notificationMessage,String path) {

        Intent myIntent = new Intent();
        File file=new File(path);
        String name = file.getName();
        int dot = name.lastIndexOf('.');
        String base = (dot == -1) ? name : name.substring(0, dot);
        String extension = (dot == -1) ? "" : name.substring(dot+1);
        String type="audio/*";
        if(extension==".mp3")
            type="audio/*";
        if(extension==".mp4")
            type="video/*";
        Intent intent = new Intent(Intent.ACTION_VIEW);
        intent.setDataAndType(Uri.fromFile(file),type );

        PendingIntent pendingIntent = PendingIntent.getActivity(
                this,
                0,
                intent,
                Intent.FLAG_ACTIVITY_NEW_TASK);
        NotificationCompat.Builder mBuilder =
                new NotificationCompat.Builder(this)
                        .setSmallIcon(R.drawable.notify_icon)
                        .setContentTitle(notificationTitle)
                        .setContentText(notificationMessage)
                        .setTicker("Notification!")
                        .setWhen(System.currentTimeMillis())
                        .setContentIntent(pendingIntent)
                        .setDefaults(Notification.DEFAULT_SOUND)
                        .setAutoCancel(true);
        NotificationManager mNotifyMgr =
                (NotificationManager) getSystemService(NOTIFICATION_SERVICE);
        mNotifyMgr.notify(IdNotdification,mBuilder.build());
        IdNotdification++;


    }






    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if(id==R.id.action_open_Transfers)
        {
            Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
            Uri uri = Uri.parse(Environment.getExternalStorageDirectory().getPath()
                    + "/download/");
            intent.setDataAndType(uri, "text/csv");
            startActivity(Intent.createChooser(intent, "Open folder"));

            return true;
        }




        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }


    public class Transfer extends AsyncTask<String, String, String> {

        String response = "";
        String dstAdress;
        int dstPort;
        Socket sock=null;

        Transfer(String Adress, int Port) {
            dstAdress = Adress;
            dstPort = Port;
        }

        public String RemoveIlegalChars(String filename)
        {
            char[] fln=filename.toCharArray();
            String flnOut="";
            for (char c:fln)
            {
              if((c>='A'&&c<='Z')||(c>='a'&&c<='z')||c=='.'||(c>='0'&&c<='9'))
                  flnOut+=c;
            }
            return flnOut.trim();
        }
        public String GetIntegerStr(String number)
        {

            char[] fln=number.toCharArray();
            String NUMOut="";
            int i=0;
            while(i<fln.length&&(fln[i]>='0'&&fln[i]<='9'))
            {
                NUMOut+=fln[i];
                if(fln[i]<'0'||fln[i]>'9')
                    return NUMOut.trim();
                i++;
            }
            return NUMOut.trim();
        }

        int fromByteArray(byte[] bytes) {
            return ByteBuffer.wrap(bytes).getInt();
        }


        @Override
        protected String doInBackground(String... params) {
            sock = null;
            try {
                publishProgress("CONULL");
                publishProgress("DISCONNECTED");
                isTransfering=true;
                //conecting
                publishProgress("Info:"+"Trying to connect with:"+dstAdress);
                sock = new Socket(dstAdress, dstPort);
                publishProgress("DISNULL");
                publishProgress("CONNECTED");
                OutputStream outputStream= sock.getOutputStream();
                publishProgress("Info:" + "Connected!");
                byte[] bytesDeviceName=new byte[128];
                String deviceName=Build.BRAND+"-"+Build.MODEL;
                byte[] deviceBytes=deviceName.getBytes("UTF-8");
                bytesDeviceName=Arrays.copyOf(deviceBytes,128);
                outputStream.write(bytesDeviceName,0,128);

                ByteArrayOutputStream byteArrayOutputStream ;
                //get the root directory
                File root = android.os.Environment.getExternalStorageDirectory();
                File dir = new File(root.getAbsolutePath() + "/download");
                dir.mkdirs();

                while (sock.isConnected())
                {
                    if(isCancelled())
                    {
                        sock.close();
                    }
                    //get input and output stream of connection
                    InputStream inputStream = sock.getInputStream();



                    //get filename

                    int bytesRead=0;
                    //buffer read filename
                    byte[] bufferFilename=new byte[128];
                    byteArrayOutputStream=new ByteArrayOutputStream(128);
                    //read filename from socket
                    bytesRead=inputStream.read(bufferFilename,0,bufferFilename.length);
                    byteArrayOutputStream.write(bufferFilename, 0, bytesRead);
                    byteArrayOutputStream.flush();
                    String filename=byteArrayOutputStream.toString("UTF-8").trim();


                    //get file size
                     bytesRead=0;
                    //buffer read file size
                    byteArrayOutputStream=new ByteArrayOutputStream(64);
                    byte[] bufferSizeOfFile=new byte[64];
                    bytesRead=inputStream.read(bufferSizeOfFile,0,bufferSizeOfFile.length);
                    byteArrayOutputStream.write(bufferSizeOfFile,0,bytesRead);
                    byteArrayOutputStream.flush();
                    String strSize=new String(bufferSizeOfFile,"UTF-8");
                    //String Totalsize=GetIntegerStr(strSize);
                    int TotalSizeInt=0;
                    String Totalsize=strSize.trim();
                    try {
                        TotalSizeInt = Integer.parseInt(Totalsize);
                   }
                    catch (Exception exc)
                    {
                        isTransfering=false;
                        publishProgress("CONULL");
                        publishProgress("DISCONNECTED");
                        response+="Eroare parsing size of file recived to integer! "+Totalsize;
                        return response;
                    }

                    publishProgress("Info:" + " Size: " + TotalSizeInt);



                    //get path of file to download...
                    File file = new File(dir, filename);
                    FileOutputStream f = new FileOutputStream(file);
                    publishProgress("Info:"+"Downloading "+filename.trim()+"...");
                    publishProgress("DOWNLOADING",filename);
                    //buffer file recived 8K
                    byte[] buffer=new byte[1024*10];
                    int nrBytesRead=0;
                    int size=TotalSizeInt;
                    while (size>0&&(bytesRead = inputStream.read(buffer,0,Math.min(buffer.length,size)))>0) {

                        f.write(buffer, 0, bytesRead);

                        publishProgress(String.valueOf((int)(((float)nrBytesRead/(float)TotalSizeInt)*100)));
                        size-=bytesRead;
                        nrBytesRead+=bytesRead;


                    }

                    publishProgress("100");
                    publishProgress("Notify:i",filename,file.getPath());
                    publishProgress("Info:" + "Recived!");
                    publishProgress("COMPLETEDOWNLOAD");


                    publishProgress("Info:"+"Recived "+filename.trim());



                }

                publishProgress("Info:"+"Transfer Succesed!");
                response="Disconected!";
                isTransfering=false;

                publishProgress("CONULL");
                publishProgress("DISCONNECTED");

            }
            catch (Exception exc)
            {
                exc.printStackTrace();
                isTransfering=false;
                publishProgress("CONULL");
                publishProgress("DISCONNECTED");

                response+=" "+exc.getMessage();
                return response;
            }


            return response;
        }



        @Override
        protected void onPostExecute(String s) {
            editText_status.append(response + "\r\n");
            super.onPostExecute(s);

        }

        @Override
        protected void onCancelled() {
            isTransfering=false;
           textView_connected.setText("");
            if(sock!=null) {
                try {
                    sock.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            textView_disconnected.setText("DISCONNECTED!");
            super.onCancelled();
        }

        @Override
        protected void onCancelled(String s) {
            isTransfering=false;
            textView_connected.setText("");
            if(sock!=null)
            {
                try {
                    sock.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            textView_disconnected.setText("DISCONNECTED!");


            super.onCancelled(s);
        }

        @Override
        protected void onProgressUpdate(String... values) {

            if(values[0]=="COMPLETEDOWNLOAD")
            {
                textView_currentTask.setText("Complete!");
                return;
            }

            if(values[0] == "DOWNLOADING") {
                textView_currentTask.setText("Donloading" + values[1]);
                return;
            }

            if(values[0]=="CONULL")
            {
                textView_connected.setText("");
                return;
            }
            if(values[0]=="DISNULL")
            {
                textView_disconnected.setText("");
                return;
            }
           if(values[0]=="CONNECTED")
           {
               textView_connected.setText("CONNECTED!");
               return;
           }

            if(values[0]=="DISCONNECTED")
            {
                textView_disconnected.setText("DISCONECTED!");
                textView_currentTask.setText("");
                isTransfering=false;
                if(sock!=null)
                    try {
                        sock.close();
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                return;
            }
            if(values[0].startsWith("Notify:"))
            {
             // editText_status.append("Notyfy\r\n");
                Notify("Download Complete",values[1],values[2]);



                return;
            }


            if(values[0].startsWith("Info:"))
            {
                editText_status.append(values[0] + "\r\n");
            }
            else{
               progressBar_transferProgress.setProgress(Integer.parseInt(values[0]));
            }

            super.onProgressUpdate(values);
        }
    }
}