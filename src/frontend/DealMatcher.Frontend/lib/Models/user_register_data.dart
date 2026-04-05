class UserRegisterData {
  final String _email;
  final String _password;
  final String _name;
  final String _surname;

  const UserRegisterData({
    required String email,
    required String password,
    required String name,
    required String surname,
  }) : _email = email,
       _password = password,
       _name = name,
       _surname = surname;

  Map<String, dynamic> toJson() => {
    'email': _email,
    'password': _password,
    'name': _name,
    'surname': _surname,
  };
}
