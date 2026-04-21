import 'package:flutter/material.dart';

class ProfileActionButtons extends StatelessWidget {
  final bool isSaving;
  final bool isDeleting;
  final bool hasChanges;
  final VoidCallback onSave;
  final VoidCallback onLogout;
  final VoidCallback onDeleteAccount;

  const ProfileActionButtons({
    super.key,
    required this.isSaving,
    required this.isDeleting,
    required this.hasChanges,
    required this.onSave,
    required this.onLogout,
    required this.onDeleteAccount,
  });

  @override
  Widget build(BuildContext context) {
    final primaryColor = Theme.of(context).primaryColor;

    return Column(
      children: [
        SizedBox(
          width: double.infinity,
          child: ElevatedButton(
            key: const Key('profileSaveButton'),
            onPressed: (isSaving || !hasChanges) ? null : onSave,
            style: ElevatedButton.styleFrom(
              backgroundColor: hasChanges ? primaryColor : Colors.white12,
              foregroundColor: hasChanges ? Colors.white : Colors.white38,
              disabledBackgroundColor: Colors.white12,
              disabledForegroundColor: Colors.white30,
            ),
            child: isSaving
                ? const SizedBox(
                    height: 18,
                    width: 18,
                    child: CircularProgressIndicator(strokeWidth: 2),
                  )
                : const Text('Zapisz zmiany'),
          ),
        ),
        const SizedBox(height: 10),
        SizedBox(
          width: double.infinity,
          child: OutlinedButton.icon(
            onPressed: isDeleting ? null : onLogout,
            icon: const Icon(Icons.logout, size: 18),
            label: const Text('Wyloguj'),
            style: OutlinedButton.styleFrom(
              foregroundColor: Colors.white70,
              side: const BorderSide(color: Colors.white24),
              padding: const EdgeInsets.symmetric(vertical: 12),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12),
              ),
            ),
          ),
        ),
        const SizedBox(height: 10),
        SizedBox(
          width: double.infinity,
          child: ElevatedButton.icon(
            onPressed: isDeleting ? null : onDeleteAccount,
            icon: isDeleting
                ? const SizedBox(
                    height: 18,
                    width: 18,
                    child: CircularProgressIndicator(
                      strokeWidth: 2,
                      color: Colors.white,
                    ),
                  )
                : const Icon(Icons.delete_forever_outlined, size: 18),
            label: isDeleting
                ? const Text('Usuwanie...')
                : const Text('Usuń konto'),
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.redAccent.shade700,
              foregroundColor: Colors.white,
              padding: const EdgeInsets.symmetric(vertical: 12),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(12),
              ),
            ),
          ),
        ),
      ],
    );
  }
}
