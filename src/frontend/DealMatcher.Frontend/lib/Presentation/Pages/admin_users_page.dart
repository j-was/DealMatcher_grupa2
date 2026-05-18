import 'package:flutter/material.dart';
import 'package:frontend/Models/admin_users_response.dart';
import 'package:frontend/Models/user.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Services/admin_service.dart';

class AdminUsersPage extends StatefulWidget {
  const AdminUsersPage({super.key});

  @override
  State<AdminUsersPage> createState() => _AdminUsersPageState();
}

class _AdminUsersPageState extends State<AdminUsersPage> {
  final AdminService _adminService = AdminService();
  final List<String> _statuses = ['_', 'ACTIVE', 'INACTIVE', 'BANNED'];
  String _selectedStatus = '_';

  bool _isLoading = false;

  List<User> _users = [];

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
      final AdminUsersResponse response = await _adminService.getUsers(
        page: _page,
        limit: _limit,
        status: _selectedStatus == '_' ? null : _selectedStatus,
      );

      setState(() {
        _users = response.items;
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

    if (_users.isEmpty) {
      return const Center(child: Text('Brak użytkowników do wyświetlenia.'));
    }

    return RefreshIndicator(
      onRefresh: _refreshUsers,
      child: Column(
        children: [
          Align(
            alignment: Alignment.centerLeft,
            child: Text(
              'Liczba użytkowników: $_total',
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
                final user = _users[index];
                return _buildUserCard(user);
              },
              separatorBuilder: (_, __) => const SizedBox(height: 8),
              itemCount: _users.length,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildUserCard(User user) => Card(
    child: ListTile(
      leading: CircleAvatar(child: Text(_getUserInitials(user))),
      title: Text(
        '${user.name} ${user.surname}',
        style: const TextStyle(
          fontWeight: FontWeight.w600,
          color: Colors.white60,
        ),
      ),
      subtitle: Padding(
        padding: const EdgeInsets.only(top: 4),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(user.email),
            const SizedBox(height: 4),
            Text('ID: ${user.id}'),
            Text('Status: ${user.status}'),
            Text('Utworzono: ${_formatDate(user.createdAt)}'),
          ],
        ),
      ),
    ),
  );

  String _getUserInitials(User user) {
    final nameInitial = user.name.isNotEmpty ? user.name[0] : '';
    final surnameInitial = user.surname.isNotEmpty ? user.surname[0] : '';

    final initials = '$nameInitial$surnameInitial'.toUpperCase();

    return initials.isNotEmpty ? initials : '?';
  }

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
                      style: TextStyle(color: Colors.white54),
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
