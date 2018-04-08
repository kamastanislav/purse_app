package com.purse.array_adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import com.purse.entity.UserData;

import java.util.List;

public class UserAdapter extends ArrayAdapter<UserData> {

    public UserAdapter(Context context, int resource, List<UserData> userDatas) {
        super(context, resource, userDatas);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        View v = convertView;

        if (v == null) {
            LayoutInflater inflater = (LayoutInflater) getContext().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            //    v = inflater.inflate(R.layout. , parent, false);
        }

        UserData userData = getItem(position);

        if (userData != null) {
       /*     TextView tvStudentId = (TextView) v.findViewById(R.id.student_Id);
            TextView tvStudentName = (TextView) v.findViewById(R.id.student_name);
            tvStudentId.setText( Integer.toString(student.Id));
            tvStudentName.setText(student.Name);*/
        }

        return v;
    }
}