import 'dart:convert';

import 'package:frontend/Models/offer.dart';
import 'package:http/http.dart' as http;

class OfferService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Future<Offer> getOffer(int offerId) async {
    final uri = Uri.parse('$baseUrl/v1/offers/$offerId');

    final response = await http.get(
      uri,
      headers: {'Accept': 'application/json'},
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map<String, dynamic>;
      return Offer.fromJson(json);
    }

    throw Exception(
      'Nie udało się pobrać oferty.'
      'Status: ${response.statusCode}',
    );
  }

  Future<Offer> createOffer(Map<String, dynamic> jsonOffer) async {
    final uri = Uri.parse('$baseUrl/v1/offers');

    final response = await http.post(
      uri,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: jsonEncode(jsonOffer),
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map<String, dynamic>;
      return Offer.fromJson(json);
    }

    throw Exception(
      'Nie udało się pobrać oferty.'
      'Status: ${response.statusCode}',
    );
  }
}
