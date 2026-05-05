import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';

class DeliveryPage extends StatefulWidget {
  const DeliveryPage({super.key});

  @override
  State<DeliveryPage> createState() => _DeliveryPageState();
}

class _DeliveryPageState extends State<DeliveryPage> {
  final _formKey = GlobalKey<FormState>();

  final _nameController = TextEditingController();
  final _surnameController = TextEditingController();
  final _streetController = TextEditingController();
  final _houseNumberController = TextEditingController();
  final _postalCodeController = TextEditingController();
  final _cityController = TextEditingController();
  final _phoneController = TextEditingController();
  final _emailController = TextEditingController();

  bool _success = false;

  @override
  void dispose() {
    _nameController.dispose();
    _surnameController.dispose();
    _streetController.dispose();
    _houseNumberController.dispose();
    _postalCodeController.dispose();
    _cityController.dispose();
    _phoneController.dispose();
    _emailController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: _success ? _successView() : _formView(),
      ),
    );
  }

  Widget _formView() {
    return Form(
      key: _formKey,
      child: ListView(
        children: [
          TextFormField(
            controller: _nameController,
            decoration: const InputDecoration(labelText: 'Imię'),
            validator: (v) =>
                (v == null || v.trim().isEmpty) ? 'Wymagane imię' : null,
          ),
          const SizedBox(height: 16),

          TextFormField(
            controller: _surnameController,
            decoration: const InputDecoration(labelText: 'Nazwisko'),
            validator: (v) =>
                (v == null || v.trim().isEmpty) ? 'Wymagane nazwisko' : null,
          ),
          const SizedBox(height: 16),
          TextFormField(
            controller: _streetController,
            decoration: const InputDecoration(labelText: 'Ulica'),
            validator: (v) =>
                (v == null || v.trim().isEmpty) ? 'Wymagana ulica' : null,
          ),
          const SizedBox(height: 16),
          TextFormField(
            controller: _houseNumberController,
            decoration: const InputDecoration(labelText: 'Numer domu'),
            validator: (v) =>
                (v == null || v.trim().isEmpty) ? 'Wymagany numer domu' : null,
          ),
          const SizedBox(height: 16),
          TextFormField(
            controller: _postalCodeController,
            decoration: const InputDecoration(labelText: 'Kod pocztowy'),
            validator: (v) {
              if (v == null || v.trim().isEmpty) return 'Wymagany kod pocztowy';
              if (!RegExp(r'^\d{2}-\d{3}$').hasMatch(v.trim())) {
                return 'Format: 00-000';
              }
              return null;
            },
          ),
          const SizedBox(height: 16),
          TextFormField(
            controller: _cityController,
            decoration: const InputDecoration(labelText: 'Miasto'),
            validator: (v) =>
                (v == null || v.trim().isEmpty) ? 'Wymagane miasto' : null,
          ),
          const SizedBox(height: 16),
          TextFormField(
            controller: _phoneController,
            decoration: const InputDecoration(labelText: 'Telefon'),
            validator: (v) =>
                (v == null || v.trim().isEmpty) ? 'Wymagany telefon' : null,
          ),
          const SizedBox(height: 16),
          TextFormField(
            controller: _emailController,
            decoration: const InputDecoration(labelText: 'E-mail'),
            validator: (v) {
              if (v == null || v.trim().isEmpty) return 'Wymagany e-mail';
              if (!v.contains('@')) return 'Niepoprawny e-mail';
              return null;
            },
          ),
          const SizedBox(height: 20),
          ElevatedButton(
            onPressed: () {
              if (_formKey.currentState?.validate() ?? false) {
                setState(() => _success = true);
              }
            },
            child: const Text('Przejdź dalej'),
          ),
        ],
      ),
    );
  }

  Widget _successView() {
    return Center(
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: const [
          Icon(Icons.check_circle_outline, size: 72),
          SizedBox(height: 16),
          Text('Udało się przejść dalej'),
        ],
      ),
    );
  }
}
