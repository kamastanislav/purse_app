package com.purse.purseclient;

import android.app.DatePickerDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.TextUtils;
import android.text.format.DateUtils;
import android.view.View;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;

import java.util.Calendar;

public class RegistrationActivity extends AppCompatActivity {

    private TextView dateBirthday;
    private Calendar dateAndTime=Calendar.getInstance();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_registration);

        Button btninsert = (Button)findViewById(R.id.btn);

        btninsert.setOnClickListener( new View.OnClickListener() {
            public void onClick(View v) {

                EditText userName = (EditText) findViewById(R.id.email);

                if( TextUtils.isEmpty(userName.getText())){

                    userName.setError( "First name is required!" );

                }



            }
        });

        dateBirthday = (TextView)findViewById(R.id.date_birthday);
        setInitialDateTime();
    }

    private void setInitialDateTime() {

        dateBirthday.setText(DateUtils.formatDateTime(this,
                dateAndTime.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
    }

    private DatePickerDialog.OnDateSetListener d=new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateAndTime.set(Calendar.YEAR, year);
            dateAndTime.set(Calendar.MONTH, monthOfYear);
            dateAndTime.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setInitialDateTime();
        }
    };

    public void setDate(View v) {
        new DatePickerDialog(RegistrationActivity.this, d,
                dateAndTime.get(Calendar.YEAR),
                dateAndTime.get(Calendar.MONTH),
                dateAndTime.get(Calendar.DAY_OF_MONTH))
                .show();
    }
}
