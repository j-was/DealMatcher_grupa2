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
    final uri = Uri.parse('$baseUrl/offers/$offerId');

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

  Future<List<Offer>> searchOffers(
    Map<String, dynamic> jsonSearchParams,
  ) async {
    final uri = Uri.parse('$baseUrl/offers/search');

    final response = await http.post(
      uri,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: jsonEncode(jsonSearchParams),
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as List<dynamic>;

      return json
          .map((item) => Offer.fromJson(item as Map<String, dynamic>))
          .toList();
    }

    throw Exception(
      'Nie udało się pobrać ofert.'
      'Status: ${response.statusCode}',
    );
  }

  Future<Offer> createOffer(
    Map<String, dynamic> jsonOffer,
    List<XFile> images,
  ) async {
    final uri = Uri.parse('$baseUrl/offers');

    final request = http.MultipartRequest('POST', uri);

    request.headers['Accept'] = 'application/json';
    request.headers['Authorization'] =
        'Bearer ${AuthService.instance.accessToken}';
    request.fields['Title'] = jsonOffer['title'].toString();
    request.fields['Description'] = jsonOffer['description'].toString();
    request.fields['Price'] = jsonOffer['price'].toString();
    request.fields['CategoryId'] = jsonOffer['categoryId'].toString();
    request.fields['Availability'] = jsonOffer['availability'].toString();
    request.fields['Tags'] = jsonEncode(jsonOffer['tags'] ?? []);
    request.fields['Properties'] = jsonEncode(jsonOffer['properties'] ?? {});
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

    if (response.statusCode == 201) {
      final json = jsonDecode(response.body) as Map<String, dynamic>;
      return Offer.fromJson(json);
    }

    if (response.statusCode == 400) {
      throw Exception(
        'Nieprawidłowe dane oferty. Sprawdź wymagane pola: tytuł, opis, cenę oraz kategorię.',
      );
    }

    if (response.statusCode == 401) {
      throw Exception(
        'Nie jesteś zalogowany lub sesja wygasła. Zaloguj się ponownie.',
      );
    }

    if (response.statusCode == 500) {
      throw Exception('Wystąpił błąd serwera. Spróbuj ponownie później.');
    }

    throw Exception(
      'Nieoczekiwany błąd'
      'Status: ${response.statusCode}',
    );
  }

  Future<List<Offer>> getMyOffers() async {
    final uri = Uri.parse('$baseUrl/users/me/offers');

    final response = await http.get(
      uri,
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as List<dynamic>;
      return json
          .map((item) => Offer.fromJson(item as Map<String, dynamic>))
          .toList();
    }

    if (response.statusCode == 204) {
      return [];
    }

    if (response.statusCode == 401) {
      await AuthService.instance.clearSession();
      throw Exception('Sesja wygasła. Zaloguj się ponownie.');
    }

    throw Exception(
      'Nie udało się pobrać Twoich ofert.'
      'Status: ${response.statusCode}',
    );
  }

  Future<Offer> updateOffer(int offerId, Map<String, dynamic> jsonOffer) async {
    final uri = Uri.parse('$baseUrl/offers/$offerId');

    final response = await http.patch(
      uri,
      headers: {
        ...AuthService.instance.authHeaders(),
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      body: jsonEncode(jsonOffer),
    );

    if (response.statusCode == 200) {
      final json = jsonDecode(response.body) as Map<String, dynamic>;
      return Offer.fromJson(json);
    }

    if (response.statusCode == 401) {
      await AuthService.instance.clearSession();
      throw Exception('Sesja wygasła. Zaloguj się ponownie.');
    }

    if (response.statusCode == 403) {
      throw Exception('Nie masz uprawnień do edycji tej oferty.');
    }

    throw Exception(
      'Nie udało się zaktualizować oferty. Status: ${response.statusCode}',
    );
  }

  Future<void> deleteOffer(int offerId) async {
    final uri = Uri.parse('$baseUrl/offers/$offerId');

    final response = await http.delete(
      uri,
      headers: AuthService.instance.authHeaders(),
    );

    if (response.statusCode == 204) {
      // Successfully deleted — no body returned.
      return;
    }

    if (response.statusCode == 401) {
      await AuthService.instance.clearSession();
      throw Exception('Sesja wygasła. Zaloguj się ponownie.');
    }

    if (response.statusCode == 403) {
      throw Exception('Nie masz uprawnień do usunięcia tej oferty.');
    }

    if (response.statusCode == 404) {
      throw Exception('Oferta nie została znaleziona.');
    }

    throw Exception(
      'Nie udało się usunąć oferty. Status: ${response.statusCode}',
    );
  }
}
