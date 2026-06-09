import 'dart:convert';

import 'package:frontend/Models/payment_method.dart';
import 'package:frontend/Models/purchase_request.dart';
import 'package:http/http.dart' as http;

class PaymentService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Future<List<PaymentMethod>> getMethods() async {
    final uri = Uri.parse('$baseUrl/purchases/payment-methods');

    final response = await http.get(
      uri,
      headers: {'Accept': 'application/json'},
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body);

      final methods = (json as List)
          .map((jsonCat) => PaymentMethod.fromJson(jsonCat))
          .toList();
      return methods;
    }

    return List.empty();
  }

  Future<String> initialize(PurchaseRequest req) async {
    final uri = Uri.parse('$baseUrl/purchases/initialize');
    final client = http.Client();

    try {
      final request = http.Request('POST', uri)
        ..headers['Accept'] = 'application/json'
        ..headers['Content-Type'] = 'application/json'
        ..body = jsonEncode(req.toJson())
        ..followRedirects = false;

      final response = await client.send(request);

      if (response.statusCode == 303) {
        final location = response.headers['location'];

        if (location == null) {
          throw Exception("Backend nie zwrócił Location");
        }

        return location;
      }

      throw Exception("Błąd ${response.statusCode}. Spróbuj ponownie");
    } finally {
      client.close();
    }
  }
}
