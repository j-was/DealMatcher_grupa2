import 'dart:convert';

import 'package:frontend/Models/user.dart';
import 'package:frontend/Models/user_register_data.dart';
import 'package:http/http.dart' as http;

class UserService {
  static const baseUrl = String.fromEnvironment('API_URL');

  Future<User> registerUser(UserRegisterData urd) async {
    final uri = Uri.parse('$baseUrl/v1/users/register');

    final response = await http.post(
      uri,
      headers: {'Accept': 'application/json'},
    );

    if (response.statusCode == 201) {
      final json = jsonDecode(response.body) as Map<String, dynamic>;
      return User.fromJson(json);
    }

    throw Exception(
      'Nie udało się zarejestrować użytkownika.'
      'Status: ${response.statusCode}',
    );
  }
}
