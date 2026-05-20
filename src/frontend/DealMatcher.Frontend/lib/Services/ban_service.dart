import 'dart:convert';
import 'dart:io';

import 'package:frontend/Models/ban.dart';
import 'package:frontend/Models/ban_request.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:http/http.dart' as http;

class BanService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Map<String, String> _headers() => AuthService.instance.authHeaders();

  Uri _buildUri(String path, [Map<String, String>? queryParameters]) {
    return Uri.parse('$baseUrl/v1$path').replace(
      queryParameters: queryParameters?.isEmpty ?? true
          ? null
          : queryParameters,
    );
  }

  Never _throwUnexpected(String action, int statusCode) {
    throw Exception('Nie udało się $action. Status: $statusCode');
  }

  Future<List<Ban>> getBans({int? userId, bool? active}) async {
    try {
      final query = <String, String>{};
      if (userId != null) {
        query['userId'] = userId.toString();
      }
      if (active != null) {
        query['active'] = active.toString();
      }

      final response = await http.get(
        _buildUri('/bans', query),
        headers: _headers(),
      );

      if (response.statusCode == 200) {
        final json = jsonDecode(response.body) as List;
        return json.map((item) => Ban.fromJson(item as Map)).toList();
      }

      if (response.statusCode == 401) {
        await AuthService.instance.clearSession();
        throw Exception('Sesja wygasła. Zaloguj się ponownie.');
      }

      if (response.statusCode == 403) {
        throw Exception('Brak uprawnień administratora.');
      }

      if (response.statusCode == 500) {
        throw Exception('Wystąpił błąd serwera podczas pobierania banów.');
      }

      _throwUnexpected('pobrać listy banów', response.statusCode);
    } on SocketException {
      throw Exception('Brak odpowiedzi serwera podczas pobierania banów.');
    } catch (error) {
      if (error is Exception) rethrow;
      throw Exception('Nie udało się pobrać banów: $error');
    }
  }

  Future<Ban> getBanDetails(int banId) async {
    try {
      final response = await http.get(
        _buildUri('/bans/$banId'),
        headers: _headers(),
      );

      if (response.statusCode == 200) {
        final json = jsonDecode(response.body) as Map;
        return Ban.fromJson(json);
      }

      if (response.statusCode == 401) {
        await AuthService.instance.clearSession();
        throw Exception('Sesja wygasła. Zaloguj się ponownie.');
      }

      if (response.statusCode == 403) {
        throw Exception('Brak uprawnień administratora.');
      }

      if (response.statusCode == 404) {
        throw Exception('Ban nie został znaleziony.');
      }

      _throwUnexpected('pobrać szczegóły bana', response.statusCode);
    } on SocketException {
      throw Exception(
        'Brak odpowiedzi serwera podczas pobierania szczegółów bana.',
      );
    } catch (error) {
      if (error is Exception) rethrow;
      throw Exception('Nie udało się pobrać szczegółów bana: $error');
    }
  }

  Future<Ban> banUser(BanRequest request) async {
    try {
      final response = await http.post(
        _buildUri('/bans'),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          ..._headers(),
        },
        body: jsonEncode(request.toJson()),
      );

      if (response.statusCode == 201) {
        final json = jsonDecode(response.body) as Map;
        return Ban.fromJson(json);
      }

      if (response.statusCode == 400) {
        throw Exception('Nieprawidłowe dane bana.');
      }

      if (response.statusCode == 401) {
        await AuthService.instance.clearSession();
        throw Exception('Sesja wygasła. Zaloguj się ponownie.');
      }

      if (response.statusCode == 403) {
        throw Exception('Brak uprawnień administratora.');
      }

      if (response.statusCode == 404) {
        throw Exception('Użytkownik nie został znaleziony.');
      }

      if (response.statusCode == 409) {
        throw Exception('Użytkownik jest już zbanowany.');
      }

      _throwUnexpected('zbanować użytkownika', response.statusCode);
    } on SocketException {
      throw Exception('Brak odpowiedzi serwera podczas banowania użytkownika.');
    } catch (error) {
      if (error is Exception) rethrow;
      throw Exception('Nie udało się zbanować użytkownika: $error');
    }
  }

  Future<void> removeBan(int banId) async {
    try {
      final response = await http.delete(
        _buildUri('/bans/$banId'),
        headers: _headers(),
      );

      if (response.statusCode == 204) {
        return;
      }

      if (response.statusCode == 401) {
        await AuthService.instance.clearSession();
        throw Exception('Sesja wygasła. Zaloguj się ponownie.');
      }

      if (response.statusCode == 403) {
        throw Exception('Brak uprawnień administratora.');
      }

      if (response.statusCode == 404) {
        throw Exception('Ban nie został znaleziony.');
      }

      _throwUnexpected('usunąć bana', response.statusCode);
    } on SocketException {
      throw Exception('Brak odpowiedzi serwera podczas usuwania bana.');
    } catch (error) {
      if (error is Exception) rethrow;
      throw Exception('Nie udało się usunąć bana: $error');
    }
  }
}
