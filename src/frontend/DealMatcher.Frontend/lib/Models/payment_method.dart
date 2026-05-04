class PaymentMethod {
  final String id;
  final String name;
  final String provider;
  final String icon;

  const PaymentMethod({
    required this.id,
    required this.name,
    required this.provider,
    required this.icon,
  });

  factory PaymentMethod.fromJson(Map<String, dynamic> json) {
    return PaymentMethod(
      id: json['id']?.toString() ?? '',
      name: json['name']?.toString() ?? '',
      provider: json['provider']?.toString() ?? '',
      icon: json['icon']?.toString() ?? '',
    );
  }
}
