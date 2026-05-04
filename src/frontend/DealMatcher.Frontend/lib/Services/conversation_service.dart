import 'dart:convert';

import 'package:frontend/Models/conversation.dart';
import 'package:frontend/Models/conversation_detail.dart';
import 'package:frontend/Models/message.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:http/http.dart' as http;

class ConversationService {
  static const String baseUrl = String.fromEnvironment('API_URL');
  final AuthService _authService = AuthService.instance;

  Map<String, String> _jsonHeaders({bool authorized = false}) {
    final headers = <String, String>{
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    };

    if (authorized) {
      headers.addAll(_authService.authHeaders());
    }

    return headers;
  }

  Future<Conversation> createConversation({
    required int offerId,
    required String initialMessage,
  }) async {
    final response = await http.post(
      Uri.parse('$baseUrl/v1/conversations'),
      headers: _jsonHeaders(authorized: true),
      body: jsonEncode({'offerId': offerId, 'initialMessage': initialMessage}),
    );

    if (response.statusCode == 201) {
      return Conversation.fromJson(jsonDecode(response.body));
    }

    if (response.statusCode == 409) {
      throw Exception('Konwersacja dla tej oferty już istnieje');
    }

    throw Exception('Błąd! Nie utworzono konwersacji');
  }

  Future<List<Conversation>> getMyConversations() async {
    final response = await http.get(
      Uri.parse('$baseUrl/v1/conversations'),
      headers: _jsonHeaders(authorized: true),
    );

    if (response.statusCode == 200) {
      final resList = jsonDecode(response.body) as List<dynamic>;

      return resList
          .map((e) => Conversation.fromJson(e as Map<String, dynamic>))
          .toList();
    }

    throw Exception('Błąd! Nie pobrano konwersacji użytkownika');
  }

  Future<ConversationDetail> getConversationDetail(int conversationId) async {
    final response = await http.get(
      Uri.parse('$baseUrl/v1/conversations/$conversationId'),
      headers: _jsonHeaders(authorized: true),
    );

    if (response.statusCode == 200) {
      return ConversationDetail.fromJson(jsonDecode(response.body));
    }

    throw Exception('Błąd! Nie pobrano szczegółów konwersacji');
  }

  Future<Message> postNewConversationMessage(
    int conversationId,
    String message,
  ) async {
    final response = await http.post(
      Uri.parse('$baseUrl/v1/conversations/$conversationId/messages'),
      headers: _jsonHeaders(authorized: true),
      body: jsonEncode({'content': message}),
    );

    if (response.statusCode == 201) {
      return Message.fromJson(jsonDecode(response.body));
    }

    throw Exception('Błąd! Nie wysłano wiadomości');
  }
}
