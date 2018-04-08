package com.purse.array_adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import com.purse.entity.Plan;
import com.purse.purseclient.R;

import java.util.List;

public class PlanAdapter extends ArrayAdapter<Plan> {

    public PlanAdapter(Context context, int resource, List<Plan> plans) {
        super(context, resource, plans);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        View v = convertView;

        if (v == null) {
            LayoutInflater inflater = (LayoutInflater) getContext().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        //    v = inflater.inflate(R.layout. , parent, false);
        }

        Plan plan = getItem(position);

        if (plan != null) {
       /*     TextView tvStudentId = (TextView) v.findViewById(R.id.student_Id);
            TextView tvStudentName = (TextView) v.findViewById(R.id.student_name);
            tvStudentId.setText( Integer.toString(student.Id));
            tvStudentName.setText(student.Name);*/
        }

        return v;
    }
}
