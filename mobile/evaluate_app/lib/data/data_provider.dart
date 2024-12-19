import 'dart:convert';
import 'package:evaluate_app/models/models.dart';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;

class DataProvider with ChangeNotifier {
  final FlutterSecureStorage _storage = const FlutterSecureStorage();
  final List<Project> _projects = [];
  bool _isLoading = false;

  List<Project> get projects => _projects;
  bool get isLoading => _isLoading;

  // Projeleri yükleme fonksiyonu
  Future<void> fetchProjects() async {
    _isLoading = true;
    notifyListeners();

    final token = await _storage.read(key: 'accessToken');
    final url = Uri.parse(AppConfig.projectTeamView);

    try {
      final response = await http.get(
        url,
        headers: {
          'Authorization': 'Bearer $token',
        },
      );

      if (response.statusCode == 200) {
        final List<dynamic> data = json.decode(response.body);
        _projects.clear();
        _projects.addAll(data.map((e) => Project.fromJson(e)).toList());
      } else {
        throw Exception('${response.statusCode}: Failed to load projects.');
      }
    } catch (error) {
      print('Error fetching projects: $error');
    } finally {
      _isLoading = false;
      notifyListeners();
    }
  }

  // Projeyi onaylama fonksiyonu
  Future<bool> approveProject(int teamId) async {
    final token = await _storage.read(key: 'accessToken');
    final url =
        Uri.parse('${AppConfig.approveProject}?teamId=$teamId&approval=true');

    try {
      final response = await http.post(
        url,
        headers: {
          'Authorization': 'Bearer $token',
        },
      );

      if (response.statusCode == 200) {
        print('Team with ID $teamId approved successfully!');
        fetchProjects(); // Projeleri yeniden yükle
        return true;
      } else {
        print(
            '${response.statusCode}: Failed to approve team with ID $teamId.');
        return false;
      }
    } catch (error) {
      print('Error approving team: $error');
      return false;
    }
  }

  // Projeyi reddetme fonksiyonu
  Future<bool> rejectProject(int teamId) async {
    final token = await _storage.read(key: 'accessToken');
    final url =
        Uri.parse('${AppConfig.approveProject}?teamId=$teamId&approval=false');

    try {
      final response = await http.post(
        url,
        headers: {
          'Authorization': 'Bearer $token',
        },
      );

      if (response.statusCode == 200) {
        print('Team with ID $teamId rejected successfully!');
        fetchProjects(); // Projeleri yeniden yükle
        return true;
      } else {
        print('${response.statusCode}: Failed to reject team with ID $teamId.');
        return false;
      }
    } catch (error) {
      print('Error rejecting team: $error');
      return false;
    }
  }
}
