class UserRegisterData {
  final String _email;
  final String _password;
  final String _name;
  final String _surname;

  const UserRegisterData({
    required String email,
    required String name,
    required String surname,
    required String password,
  }) : _email = email,
       _name = name,
       _surname = surname,
       _password = password;

  Map<String, dynamic> toJson() => {
    'email': _email,
    'name': _name,
    'surname': _surname,
    'password': _password,
  };
}
