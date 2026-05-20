import 'package:flutter/material.dart';
import 'package:frontend/Models/ban.dart';

class BanListTile extends StatelessWidget {
  final Ban ban;
  final bool selected;
  final VoidCallback onTap;

  const BanListTile({
    super.key,
    required this.ban,
    required this.selected,
    required this.onTap,
  });

  String _formatDate(DateTime value) {
    return value.toLocal().toString().split('.').first;
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: selected ? 4 : 1,
      child: ListTile(
        onTap: onTap,
        selected: selected,
        leading: const CircleAvatar(child: Icon(Icons.block)),
        title: Text('Użytkownik #${ban.userId}'),
        subtitle: Text(
          '${ban.reason}\nWystawiono: ${_formatDate(ban.issuedAt)}',
        ),
        isThreeLine: true,
        trailing: selected
            ? const Icon(Icons.chevron_right)
            : const Icon(Icons.chevron_right_outlined),
      ),
    );
  }
}
