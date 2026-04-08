import 'package:frontend/Models/user.dart';

class LoginResponse {
  final String accessToken;
  final User user;

  const LoginResponse({required this.accessToken, required this.user});

  factory LoginResponse.fromJson(Map<String, dynamic> json) {
    return LoginResponse(
      accessToken: json['accessToken'] as String,
      user: User.fromJson(json['user'] as Map<String, dynamic>),
    );
  }
}
