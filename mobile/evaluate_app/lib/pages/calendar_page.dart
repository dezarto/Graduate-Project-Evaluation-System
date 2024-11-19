import 'package:flutter/material.dart';
import 'package:table_calendar/table_calendar.dart';
import 'package:evaluate_app/resources/app_resources.dart';

class CalendarScreen extends StatefulWidget {
  const CalendarScreen({Key? key}) : super(key: key);

  @override
  _CalendarScreenState createState() => _CalendarScreenState();
}

class _CalendarScreenState extends State<CalendarScreen> {
  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Calendar'),
        backgroundColor: AppColors.primary,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 30,
          fontWeight: FontWeight.bold,
        ),
      ),
      body: Container(
        decoration: BoxDecoration(color: AppColors.pageBackground),
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            Container(
              decoration: BoxDecoration(
                color: AppColors.whiteTextColor,
                borderRadius: BorderRadius.circular(20),
              ),
              child: TableCalendar(
                firstDay: DateTime.utc(2020, 1, 1),
                lastDay: DateTime.utc(2030, 12, 31),
                focusedDay: _focusedDay,
                selectedDayPredicate: (day) => isSameDay(_selectedDay, day),
                onDaySelected: (selectedDay, focusedDay) {
                  setState(() {
                    _selectedDay = selectedDay;
                    _focusedDay = focusedDay;
                  });
                },
                calendarStyle: CalendarStyle(
                  selectedDecoration: BoxDecoration(
                    color: Colors.blue,
                    shape: BoxShape.circle,
                  ),
                  todayDecoration: BoxDecoration(
                    color: Colors.orange,
                    shape: BoxShape.circle,
                  ),
                  todayTextStyle: const TextStyle(color: Colors.white),
                ),
                headerStyle: const HeaderStyle(
                  formatButtonVisible: false,
                  titleCentered: true,
                ),
              ),
            ),
            const SizedBox(height: 20),
            if (_selectedDay != null)
              Text(
                'Selected Date: ${_selectedDay!.toLocal()}'.split(' ')[0],
                style: const TextStyle(fontSize: 18),
              )
            else
              const Text('Henüz bir tarih seçilmedi.',
                  style: TextStyle(fontSize: 18)),
          ],
        ),
      ),
    );
  }
}
