class UserUpdateData {
  final String name;
  final String surname;

  const UserUpdateData({required this.name, required this.surname});

  Map<String, dynamic> toJson() => {'name': name, 'surname': surname};
}
