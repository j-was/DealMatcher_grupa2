import 'package:flutter/material.dart';
import 'package:frontend/Models/ban.dart';
import 'package:frontend/Models/user.dart';
import 'package:frontend/Services/ban_service.dart';

class BanDetailCard extends StatelessWidget {
  final Ban? ban;
  final User? user;
  final bool loadingUser;
  final BanService _banService = BanService();

  BanDetailCard({
    super.key,
    required this.ban,
    required this.user,
    required this.loadingUser,
  });

  String _formatDate(DateTime value) {
    return value.toLocal().toString().split('.').first;
  }

  Widget _infoRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 12),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 120,
            child: Text(
              label,
              style: const TextStyle(fontWeight: FontWeight.w600),
            ),
          ),
          Expanded(child: Text(value)),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (ban == null) {
      return const Card(
        child: Padding(
          padding: EdgeInsets.all(24),
          child: Text('Wybierz bana z listy, aby zobaczyć szczegóły.'),
        ),
      );
    }

    return Card(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          mainAxisSize: MainAxisSize.min,
          children: [
            Row(
              children: [
                const Icon(Icons.gpp_bad, size: 28),
                const SizedBox(width: 12),
                Expanded(
                  child: Text(
                    user == null
                        ? 'Ban #${ban!.id}'
                        : '${user!.name} ${user!.surname}',
                    style: Theme.of(context).textTheme.titleLarge,
                  ),
                ),
                Chip(label: Text(ban!.isActive ? 'Aktywny' : 'Nieaktywny')),
              ],
            ),
            const SizedBox(height: 20),
            if (loadingUser)
              const LinearProgressIndicator()
            else ...[
              _infoRow('ID bana', ban!.id.toString()),
              _infoRow('ID użytkownika', ban!.userId.toString()),
              _infoRow(
                'Użytkownik',
                user == null
                    ? 'Brak danych użytkownika'
                    : '${user!.email} (${user!.status})',
              ),
              _infoRow('Powód', ban!.reason),
              _infoRow('Wystawił', ban!.issuedBy.toString()),
              _infoRow('Wystawiono', _formatDate(ban!.issuedAt)),
              _infoRow(
                'Wygasa',
                ban!.expiresAt == null ? 'Nigdy' : _formatDate(ban!.expiresAt!),
              ),
              Center(
                child: TextButton(
                  onPressed: () async {
                    await _banService.removeBan(ban!.id);
                  },
                  child: Text(
                    "Odbanuj użytkownika",
                    style: TextStyle(fontSize: 16, color: Colors.redAccent),
                  ),
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
}
