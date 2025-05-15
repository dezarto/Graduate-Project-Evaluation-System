import 'package:flutter/material.dart';
import 'package:evaluate_app/pages/calendar_details.dart';
import 'package:evaluate_app/pages/calendar_page.dart';

class Calendar extends StatefulWidget {
  const Calendar({super.key});

  @override
  CalendarState createState() => CalendarState();
}

GlobalKey<NavigatorState> calendarNavigatorKey = GlobalKey<NavigatorState>();

class CalendarState extends State<Calendar> {
  @override
  Widget build(BuildContext context) {
    return Navigator(
      key: calendarNavigatorKey,
      onGenerateRoute: (RouteSettings settings) {
        return MaterialPageRoute(
            settings: settings,
            builder: (BuildContext context) {
              if (settings.name == "/detailsCalendar") {
                return CalendarDetails();
              }
              return CalendarScreen();
            });
      },
    );
  }
}
