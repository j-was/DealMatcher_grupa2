import 'package:flutter/material.dart';
import 'package:frontend/Models/admin_offers_response.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Services/admin_service.dart';
import 'package:go_router/go_router.dart';

class AdminOffersPage extends StatefulWidget {
  const AdminOffersPage({super.key});

  @override
  State<AdminOffersPage> createState() => _AdminOffersPageState();
}

class _AdminOffersPageState extends State<AdminOffersPage> {
  final AdminService _adminService = AdminService();
  final List<String> _statuses = [
    '_',
    'DRAFT',
    'ACTIVE',
    'PROMOTED',
    'SOLD',
    'DELETED',
  ];
  String _selectedStatus = '_';

  bool _isLoading = false;

  List<Offer> _offers = [];

  int _page = 1;
  final int _limit = 20;
  int _total = 0;
  int _pages = 1;

  String? _errorMessage;

  @override
  void initState() {
    super.initState();
    _loadUsers();
  }

  Future<void> _loadUsers() async {
    setState(() {
      _isLoading = true;
    });

    try {
      final AdminOffersResponse response = await _adminService.getOffers(
        page: _page,
        limit: _limit,
        status: _selectedStatus == '_' ? null : _selectedStatus,
      );

      setState(() {
        _offers = response.items;
        _total = response.total;
        _page = response.page;
        _pages = response.pages;
      });
    } catch (error) {
      setState(() {
        _errorMessage = error.toString();
      });
    } finally {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    }
  }

  void _onStatusChanged(String? status) {
    if (status == null) {
      return;
    }

    setState(() {
      _selectedStatus = status;
      _page = 1;
    });

    _loadUsers();
  }

  Future<void> _refreshUsers() async {
    await _loadUsers();
  }

  Widget _buildContent() {
    if (_isLoading) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_errorMessage != null) {
      return Center(
        child: Text(
          _errorMessage!,
          textAlign: TextAlign.center,
          style: const TextStyle(color: Colors.red),
        ),
      );
    }

    if (_offers.isEmpty) {
      return const Center(child: Text('Brak ofert do wyświetlenia.'));
    }

    return RefreshIndicator(
      onRefresh: _refreshUsers,
      child: Column(
        children: [
          Align(
            alignment: Alignment.centerLeft,
            child: Text(
              'Liczba ofert: $_total',
              style: const TextStyle(
                fontWeight: FontWeight.w600,
                color: Colors.white60,
              ),
            ),
          ),
          const SizedBox(height: 8),
          Expanded(
            child: ListView.separated(
              itemBuilder: (context, index) {
                final offer = _offers[index];
                return _buildOfferCard(offer);
              },
              separatorBuilder: (_, __) => const SizedBox(height: 8),
              itemCount: _offers.length,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildOfferCard(Offer offer) => Card(
    color: Colors.amber,
    child: Stack(
      children: [
        ListTile(
          leading: CircleAvatar(
            child: Text(
              offer.id.toString(),
              style: const TextStyle(fontSize: 12),
            ),
          ),
          title: Text(
            offer.title,
            style: const TextStyle(
              fontWeight: FontWeight.w600,
              color: Colors.white70,
            ),
          ),
          subtitle: Padding(
            padding: const EdgeInsets.only(top: 4),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('ID: ${offer.id}'),
                Text('Cena: ${offer.price}'),
                Text('Status: ${offer.status}'),
                Text('Utworzono: ${_formatDate(offer.createdAt)}'),
              ],
            ),
          ),
        ),
        Positioned(
          top: 4,
          right: 4,
          child: TextButton(
            child: Text("Aktywność oferty"),
            onPressed: () {
              context.go('admin/activity/offer/${offer.id}');
            },
          ),
        ),
      ],
    ),
  );

  String _formatDate(DateTime date) {
    final localDate = date.toLocal();

    final day = localDate.day.toString().padLeft(2, '0');
    final month = localDate.month.toString().padLeft(2, '0');
    final year = localDate.year.toString();

    return '$day.$month.$year';
  }

  @override
  Widget build(BuildContext context) => Scaffold(
    appBar: MainAppBar(),
    body: Padding(
      padding: const EdgeInsets.all(16),
      child: Column(
        children: [
          DropdownButtonFormField<String>(
            initialValue: _selectedStatus,
            dropdownColor: const Color(0xFF2B2B2B),
            decoration: const InputDecoration(
              labelText: 'Status użytkowników',
              border: OutlineInputBorder(),
            ),
            items: _statuses
                .map(
                  (status) => DropdownMenuItem<String>(
                    value: status,
                    child: Text(
                      status,
                      style: TextStyle(color: Colors.white60),
                    ),
                  ),
                )
                .toList(),
            onChanged: _isLoading ? null : _onStatusChanged,
          ),

          const SizedBox(height: 16),
          Expanded(child: _buildContent()),
          const SizedBox(height: 16),
          Center(child: Text('$_page/$_pages')),
        ],
      ),
    ),
  );
}
