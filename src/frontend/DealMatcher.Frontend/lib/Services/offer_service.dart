import 'dart:convert';
import 'package:http_parser/http_parser.dart';
import 'package:frontend/Models/offer.dart';
import 'package:http/http.dart' as http;
import 'package:image_picker/image_picker.dart';
import 'package:path/path.dart' as path;
import 'package:frontend/Services/auth_service.dart';

class OfferService {
  static const String baseUrl = String.fromEnvironment('API_URL');

  Future<Offer> getOffer(int offerId) async {
    final uri = Uri.parse('$baseUrl/v1/offers/$offerId');

    final request = await http.get(
      uri,
      headers: {'Accept': 'application/json'},
    );

    if (request.statusCode == 200) {
      final json = jsonDecode(request.body) as Map<String, dynamic>;
      return Offer.fromJson(json);
    }

    throw Exception(
      'Nie udało się pobrać oferty.'
      'Status: ${request.statusCode}',
    );
  }

  Future<Offer> createOffer(
    Map<String, dynamic> jsonOffer,
    List<XFile> images,
  ) async {
    final uri = Uri.parse('$baseUrl/v1/offers');

    final request = http.MultipartRequest('POST', uri);

    request.headers['Accept'] = 'application/json';
    request.headers['Authorization'] =
        'Bearer ${AuthService.instance.accessToken}';
    request.fields['data'] = jsonEncode(jsonOffer);

    for (final img in images) {
      final bytes = await img.readAsBytes();

      final originalName = img.name;
      var extension = path
          .extension(originalName)
          .replaceFirst('.', '')
          .toLowerCase();

      final filename = path.extension(originalName).isEmpty
          ? '${DateTime.now().millisecondsSinceEpoch}.$extension'
          : originalName;

      request.files.add(
        http.MultipartFile.fromBytes(
          'images',
          bytes,
          filename: filename,
          contentType: MediaType('image', extension),
        ),
      );
    }

    final streamedResponse = await request.send();
    final response = await http.Response.fromStream(streamedResponse);

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
