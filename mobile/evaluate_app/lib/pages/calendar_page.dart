import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:intl/intl.dart';
import 'package:evaluate_app/models/models.dart';
import 'package:evaluate_app/pages/profile.dart';

class CalendarScreen extends StatefulWidget {
  const CalendarScreen({Key? key}) : super(key: key);

  @override
  _CalendarScreenState createState() => _CalendarScreenState();
}

class _CalendarScreenState extends State<CalendarScreen> {
  DateTime? _selectedDate;
  TimeOfDay? _startTime;
  TimeOfDay? _endTime;
  User? _user;

  List<Map<String, dynamic>> _availableTimes = [];

  Future<void> _initializeUser() async {
    try {
      _user = await fetchUser();
      setState(() {});
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text("Error fetching user data!")),
      );
    }
  }

  Future<void> fetchTimes() async {
    final storage = const FlutterSecureStorage();
    final token = await storage.read(key: 'accessToken');

    try {
      final response = await http.get(
        Uri.parse(AppConfig.getAvailTimes),
        headers: {
          'Authorization': 'Bearer $token',
        },
      );

      if (response.statusCode == 200) {
        final List<dynamic> data = jsonDecode(response.body);
        setState(() {
          _availableTimes = data.map((item) {
            return {
              "availabilityId": item["availabilityId"],
              "date": DateTime.parse(item["availableDate"]),
              "startTime": TimeOfDay(
                hour: int.parse(item["startTime"].split(":")[0]),
                minute: int.parse(item["startTime"].split(":")[1]),
              ),
              "endTime": TimeOfDay(
                hour: int.parse(item["endTime"].split(":")[0]),
                minute: int.parse(item["endTime"].split(":")[1]),
              ),
            };
          }).toList();
        });
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text("Failed to fetch availability!"),
            backgroundColor: AppColors.falseRed,
          ),
        );
        print('Status Code: ${response.statusCode}');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text("Error connecting to the server!"),
          backgroundColor: AppColors.falseRed,
        ),
      );
    }
  }

  Future<void> _sendAvailability() async {
    if (_user == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text("User data not available!")),
      );
      return;
    }

    final storage = const FlutterSecureStorage();
    final token = await storage.read(key: 'accessToken');

    if (_selectedDate != null && _startTime != null && _endTime != null) {
      try {
        final availabilityData = [
          {
            "availabilityId": 0,
            "professorId": _user!.professorId,
            "availableDate":
                "${_selectedDate!.toIso8601String().split('T')[0]}T00:00:00",
            "startTime":
                "${_startTime!.hour.toString().padLeft(2, '0')}:${_startTime!.minute.toString().padLeft(2, '0')}:00",
            "endTime":
                "${_endTime!.hour.toString().padLeft(2, '0')}:${_endTime!.minute.toString().padLeft(2, '0')}:00",
          }
        ];

        final response = await http.post(
          Uri.parse(AppConfig.availableTime),
          headers: {
            "Content-Type": "application/json",
            'Authorization': 'Bearer $token',
          },
          body: jsonEncode(availabilityData),
        );

        if (response.statusCode == 200 || response.statusCode == 201) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text("Availability Added Successfully!"),
              backgroundColor: AppColors.trueGreen,
            ),
          );
          fetchTimes(); // Mevcut zamanları yenile
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text("Failed to add availability!"),
              backgroundColor: AppColors.falseRed,
            ),
          );
          print('Status Code: ${response.statusCode}, ${response.body}');
          print(availabilityData);
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text("Error connecting to the server!"),
            backgroundColor: AppColors.falseRed,
          ),
        );
      }
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text("Please select date and time!")),
      );
    }
  }

  Future<void> _deleteAvailability(int availabilityId) async {
    final storage = const FlutterSecureStorage();
    final token = await storage.read(key: 'accessToken');

    final confirmed = await showDialog<bool>(
          context: context,
          builder: (context) => AlertDialog(
            title: const Text("Confirm Deletion"),
            content: const Text(
                "Are you sure you want to delete this availability? This operation can not be undone."),
            actions: [
              TextButton(
                onPressed: () => Navigator.of(context).pop(false),
                child: const Text("Cancel"),
              ),
              TextButton(
                onPressed: () => Navigator.of(context).pop(true),
                child: const Text("Delete"),
              ),
            ],
          ),
        ) ??
        false;

    if (confirmed) {
      try {
        final response = await http.delete(
          Uri.parse("${AppConfig.deleteAvailTimes}/$availabilityId"),
          headers: {
            'Authorization': 'Bearer $token',
          },
        );

        if (response.statusCode == 200) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text("Availability Deleted Successfully!"),
              backgroundColor: AppColors.trueGreen,
            ),
          );

          // Refresh available times
          fetchTimes();
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text("Failed to delete availability!"),
              backgroundColor: AppColors.falseRed,
            ),
          );
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text("Error connecting to the server!"),
            backgroundColor: AppColors.falseRed,
          ),
        );
      }
    }
  }

  Future<void> _pickDate() async {
    final DateTime? pickedDate = await showDatePicker(
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

  Future<TimeOfDay?> _pickTime() async {
    return await showTimePicker(
      context: context,
      initialTime: const TimeOfDay(hour: 9, minute: 0),
    );
  }

  @override
  void initState() {
    super.initState();
    fetchTimes();
    _initializeUser();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        scrolledUnderElevation: 0,
        automaticallyImplyLeading: false,
        title: const Text('Calendar'),
        backgroundColor: AppColors.primary,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 36,
          fontWeight: FontWeight.bold,
        ),
        leading: null,
        toolbarHeight: 60,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.vertical(
            bottom: Radius.circular(20),
          ),
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
            const SizedBox(height: 8),
            Container(
              decoration: BoxDecoration(
                color: AppColors.whiteTextColor,
                borderRadius: BorderRadius.circular(20.0),
              ),
              child: Column(
                children: [
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
                  const SizedBox(height: 8),
                  ElevatedButton(
                    onPressed: _sendAvailability,
                    child: const Text('Add Availability'),
                  ),
                  const SizedBox(height: 8),
                ],
              ),
            ),
            const SizedBox(height: 8),
            const Text(
              'Available Times',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: AppColors.primaryTextColor,
              ),
            ),
            Expanded(
              child: ListView.builder(
                itemCount: _availableTimes.length,
                itemBuilder: (context, index) {
                  final availability = _availableTimes[index];
                  return Container(
                    child: ListTile(
                      title: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            '${DateFormat.yMMMd().format(availability['date'])}',
                            style: TextStyle(
                                fontWeight: FontWeight.bold, fontSize: 20),
                          ),
                          Text(
                              '${availability['startTime'].format(context)} - ${availability['endTime'].format(context)}'),
                        ],
                      ),
                      trailing: IconButton(
                        icon: const Icon(
                          Icons.delete,
                          color: AppColors.falseRed,
                        ),
                        onPressed: () =>
                            _deleteAvailability(availability['availabilityId']),
                      ),
                    ),
                  );
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
