import 'package:flutter/material.dart';
import 'package:frontend/Models/user.dart';
import 'package:frontend/Models/user_update_data.dart';
import 'package:frontend/Presentation/Widgets/profile_action_buttons.dart';
import 'package:frontend/Presentation/Widgets/profile_avatar_header.dart';
import 'package:frontend/Presentation/Widgets/profile_edit_section.dart';
import 'package:frontend/Presentation/Widgets/profile_info_section.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:frontend/Services/user_service.dart';
import 'package:go_router/go_router.dart';

class ProfileForm extends StatefulWidget {
  const ProfileForm({super.key});

  @override
  State<ProfileForm> createState() => _ProfileFormState();
}

class _ProfileFormState extends State<ProfileForm> {
  final _userService = UserService();
  final _formKey = GlobalKey<FormState>();

  final _nameController = TextEditingController();
  final _surnameController = TextEditingController();

  String _email = '';
  String _status = '';
  String _createdAt = '';

  // Zapamiętane wartości po ostatnim zapisie — do porównania zmian
  String _savedName = '';
  String _savedSurname = '';

  bool _isLoading = true;
  bool _isSaving = false;
  bool _isDeleting = false;
  String? _errorMessage;

  bool get _hasChanges =>
      _nameController.text.trim() != _savedName ||
      _surnameController.text.trim() != _savedSurname;

  @override
  void initState() {
    super.initState();
    _nameController.addListener(_onFieldChanged);
    _surnameController.addListener(_onFieldChanged);
    final cached = AuthService.instance.currentUser;
    if (cached != null) _applyUser(cached);
    _loadProfile();
  }

  void _onFieldChanged() => setState(() {});

  @override
  void dispose() {
    _nameController.dispose();
    _surnameController.dispose();
    super.dispose();
  }

  String _extractMessage(Object error) => error
      .toString()
      .replaceFirst('Exception: ', '')
      .replaceFirst('Bad state: ', '');

  String _formatDate(DateTime value) =>
      value.toLocal().toString().split('.').first;

  void _applyUser(User user) {
    _nameController.text = user.name;
    _surnameController.text = user.surname;
    _savedName = user.name.trim();
    _savedSurname = user.surname.trim();
    _email = user.email;
    _status = user.status;
    _createdAt = _formatDate(user.createdAt);
  }

  Future<void> _loadProfile() async {
    setState(() {
      _isLoading = true;
      _errorMessage = null;
    });

    try {
      final user = await _userService.getCurrentUser();
      if (!mounted) return;
      setState(() {
        _applyUser(user);
        _isLoading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _isLoading = false;
        _errorMessage = _extractMessage(e);
      });
    }
  }

  Future<void> _saveProfile() async {
    if (!(_formKey.currentState?.validate() ?? false)) return;

    setState(() {
      _isSaving = true;
      _errorMessage = null;
    });

    try {
      final updatedUser = await _userService.updateCurrentUser(
        UserUpdateData(
          name: _nameController.text.trim(),
          surname: _surnameController.text.trim(),
        ),
      );
      if (!mounted) return;
      setState(() => _applyUser(updatedUser));
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Profil zapisany')));
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(_extractMessage(e))));
    } finally {
      if (mounted) setState(() => _isSaving = false);
    }
  }

  Future<void> _logout() async {
    await AuthService.instance.clearSession();
    if (!mounted) return;
    context.go('/login');
  }

  Future<void> _deleteAccount() async {
    final confirm = await showDialog<bool>(
      context: context,
      builder: (ctx) => AlertDialog(
        title: const Text('Usuń konto'),
        content: const Text(
          'To usunie konto i wyloguje Cię z aplikacji. Operacja jest nieodwracalna.',
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(ctx).pop(false),
            child: const Text('Anuluj'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.of(ctx).pop(true),
            style: ElevatedButton.styleFrom(backgroundColor: Colors.redAccent),
            child: const Text('Usuń'),
          ),
        ],
      ),
    );

    if (confirm != true) return;

    setState(() {
      _isDeleting = true;
      _errorMessage = null;
    });

    try {
      await _userService.deleteCurrentUser();
      if (!mounted) return;
      context.go('/login');
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(_extractMessage(e))));
    } finally {
      if (mounted) setState(() => _isDeleting = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_errorMessage != null && AuthService.instance.currentUser == null) {
      return Center(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(_errorMessage!),
            const SizedBox(height: 16),
            ElevatedButton(
              onPressed: _loadProfile,
              child: const Text('Spróbuj ponownie'),
            ),
          ],
        ),
      );
    }

    return Form(
      key: _formKey,
      child: ListView(
        children: [
          ProfileAvatarHeader(
            name: _nameController.text,
            surname: _surnameController.text,
          ),
          ProfileInfoSection(
            email: _email,
            status: _status,
            createdAt: _createdAt,
          ),
          const SizedBox(height: 28),
          ProfileEditSection(
            nameController: _nameController,
            surnameController: _surnameController,
          ),
          const SizedBox(height: 28),
          ProfileActionButtons(
            isSaving: _isSaving,
            isDeleting: _isDeleting,
            hasChanges: _hasChanges,
            onSave: _saveProfile,
            onLogout: _logout,
            onDeleteAccount: _deleteAccount,
          ),
          const SizedBox(height: 24),
        ],
      ),
    );
  }
}
