import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:evaluate_app/models/models.dart';

class CalendarScreen extends StatefulWidget {
  const CalendarScreen({Key? key}) : super(key: key);

  @override
  _CalendarScreenState createState() => _CalendarScreenState();
}

Future<User> fetchUser() async {
  final url = Uri.parse(AppConfig.profileView);
  final storage = const FlutterSecureStorage();
  final token = await storage.read(key: 'accessToken');

  try {
    final response = await await http.get(
      url,
      headers: {
        'Authorization': 'Bearer $token',
      },
    );

    if (response.statusCode == 200) {
      print('${response.statusCode}: User info fetched successfully!');
      final data = json.decode(response.body);
      return User(
        professorId: data['professorId'],
        fullName: data['fullName'],
        department: data['department'],
        mailAddress: data['mailAddress'],
        role: data['role'],
      );
    } else {
      print('${response.statusCode}: User info could not fetched.');
      throw Exception('Failed to load user data');
    }
  } catch (e) {
    throw Exception('Error: $e');
  }
}

class _CalendarScreenState extends State<CalendarScreen> {
  DateTime? _selectedDate;
  TimeOfDay? _startTime;
  TimeOfDay? _endTime;

  // JSON verisini endpoint'e POST isteği ile gönder
  Future<void> _sendAvailability() async {
    if (_selectedDate != null && _startTime != null && _endTime != null) {
      try {
        // Fetch user data to get professorId
        final user = await fetchUser();
        final professorId = user.professorId;

        final availabilityData = [
          {
            "availabilityId": 0,
            "professorId": 0, // Use professorId here
            "availableDate":
                "${_selectedDate!.toIso8601String().split('T')[0]}T00:00:00",
            "startTime":
                "${_startTime!.hour.toString().padLeft(2, '0')}:${_startTime!.minute.toString().padLeft(2, '0')}:00",
            "endTime":
                "${_endTime!.hour.toString().padLeft(2, '0')}:${_endTime!.minute.toString().padLeft(2, '0')}:00",
          }
        ];

        print(availabilityData);

        final response = await http.post(
          Uri.parse(AppConfig.availableTime),
          headers: {
            "Content-Type": "application/json",
          },
          body: jsonEncode(availabilityData),
        );

        if (response.statusCode == 200 || response.statusCode == 201) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text("Availability Added Successfully!")),
          );
          print("Response: ${response.body}");
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text("Failed to add availability!")),
          );
          print("Error: ${response.statusCode}, ${response.body}");
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Error connecting to the server!")),
        );
        print("Exception: $e");
      }
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text("Please select date and time!")),
      );
    }
  }

  // Tarih seçici
  Future<void> _pickDate() async {
    final DateTime? pickedDate = await showDatePicker(
      barrierColor: AppColors.primary,
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime.now(),
      lastDate: DateTime(2100),
    );

    if (pickedDate != null) {
      setState(() {
        _selectedDate = pickedDate;
      });
    }
  }

  // Saat seçici
  Future<TimeOfDay?> _pickTime() async {
    return await showTimePicker(
      context: context,
      initialTime: const TimeOfDay(hour: 9, minute: 0),
    );
  }

  // Arayüz
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Calendar'),
        backgroundColor: AppColors.primary,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 36,
          fontWeight: FontWeight.bold,
        ),
      ),
      body: Container(
        padding: const EdgeInsets.all(16.0),
        decoration: BoxDecoration(color: AppColors.pageBackground),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Add Availability',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: AppColors.primaryTextColor,
              ),
            ),
            const SizedBox(height: 16),
            // Tarih Seçici Butonu
            ListTile(
              title: Text(
                _selectedDate == null
                    ? 'Select Date'
                    : 'Date: ${_selectedDate!.toLocal()}'.split(' ')[0],
              ),
              trailing: const Icon(Icons.calendar_today),
              onTap: _pickDate,
            ),
            const SizedBox(height: 8),
            // Başlangıç Saati Seçici
            ListTile(
              title: Text(
                _startTime == null
                    ? 'Select Start Time'
                    : 'Start Time: ${_startTime!.format(context)}',
              ),
              trailing: const Icon(Icons.access_time),
              onTap: () async {
                final time = await _pickTime();
                if (time != null) {
                  setState(() {
                    _startTime = time;
                  });
                }
              },
            ),
            // Bitiş Saati Seçici
            ListTile(
              title: Text(
                _endTime == null
                    ? 'Select End Time'
                    : 'End Time: ${_endTime!.format(context)}',
              ),
              trailing: const Icon(Icons.access_time),
              onTap: () async {
                final time = await _pickTime();
                if (time != null) {
                  setState(() {
                    _endTime = time;
                  });
                }
              },
            ),
            const SizedBox(height: 20),
            // "Add Available Time" Butonu
            Center(
              child: ElevatedButton(
                onPressed: _sendAvailability,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.primary,
                  padding:
                      const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
                ),
                child: const Text(
                  'Add Available Time',
                  style: TextStyle(
                    color: AppColors.whiteTextColor,
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ),
            ),
            const SizedBox(height: 20),
            const Text(
              'My Available Times',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: AppColors.primaryTextColor,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
