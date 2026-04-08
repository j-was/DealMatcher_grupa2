class UserLoginData {
  final String _email;
  final String _password;

  const UserLoginData({required String email, required String password})
    : _email = email,
      _password = password;

  Map<String, dynamic> toJson() => {'email': _email, 'password': _password};
}
