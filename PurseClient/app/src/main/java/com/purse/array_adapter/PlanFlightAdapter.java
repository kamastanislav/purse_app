package com.purse.array_adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.purse.entity.Flight;
import com.purse.purseclient.R;

import java.util.Date;
import java.util.List;

public class PlanFlightAdapter extends ArrayAdapter<Flight> {

    public PlanFlightAdapter(Context context, int resource, List<Flight> flights) {
        super(context, resource, flights);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        View v = convertView;

        if (v == null) {
            LayoutInflater inflater = (LayoutInflater) getContext().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.view_flights_entity , parent, false);
        }

        Flight flight = getItem(position);

        if (flight != null) {
            TextView fieldFlightCode = (TextView) v.findViewById(R.id.flight_entity_code);
            TextView fieldFlightComment = (TextView) v.findViewById(R.id.flight_entity_comment);
            TextView fieldFlightDate = (TextView)v.findViewById(R.id.flight_entity_date);
            fieldFlightCode.setText(String.valueOf(flight.Code));
            fieldFlightComment.setText(flight.Comment);
            Date date = new Date(flight.DateCreate);
            fieldFlightDate.setText(date.toString());
        }

        return v;
    }
}