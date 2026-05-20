import 'package:flutter/material.dart';
import 'package:frontend/Models/activity_record.dart';

class AdminActivitySection extends StatelessWidget {
  final List<ActivityRecord> records;
  final String emptyMessage;

  const AdminActivitySection({
    super.key,
    required this.records,
    this.emptyMessage = 'Brak aktywności.',
  });

  String _formatDateTime(DateTime dateTime) {
    final local = dateTime.toLocal().toIso8601String();
    return local.replaceFirst('T', ' ').substring(0, 19);
  }

  String _formatAction(String action) {
    return action.replaceAll('_', ' ');
  }

  Widget _infoRow(
    BuildContext context, {
    required String label,
    required String value,
  }) {
    final theme = Theme.of(context);
    return Padding(
      padding: const EdgeInsets.only(bottom: 8),
      child: RichText(
        text: TextSpan(
          style: theme.textTheme.bodyMedium,
          children: [
            TextSpan(
              text: '$label: ',
              style: const TextStyle(fontWeight: FontWeight.w600),
            ),
            TextSpan(text: value),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (records.isEmpty) {
      return Center(
        child: Text(
          emptyMessage,
          style: Theme.of(context).textTheme.bodyLarge,
          textAlign: TextAlign.center,
        ),
      );
    }

    return ListView.separated(
      itemCount: records.length,
      separatorBuilder: (_, __) => const SizedBox(height: 12),
      itemBuilder: (context, index) {
        final record = records[index];

        return Card(
          elevation: 2,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16),
          ),
          child: ExpansionTile(
            tilePadding: const EdgeInsets.symmetric(
              horizontal: 16,
              vertical: 8,
            ),
            childrenPadding: const EdgeInsets.fromLTRB(16, 0, 16, 16),
            title: Text(
              _formatAction(record.action),
              style: const TextStyle(fontWeight: FontWeight.w600),
            ),
            subtitle: Text(
              'Użytkownik #${record.userId}'
              '${record.offerId != null ? ' • Oferta #${record.offerId}' : ''}',
            ),
            trailing: Text(
              _formatDateTime(record.createdAt),
              textAlign: TextAlign.right,
              style: Theme.of(context).textTheme.bodySmall,
            ),
            children: [
              _infoRow(context, label: 'ID rekordu', value: '${record.id}'),
              _infoRow(context, label: 'IP', value: record.ipAddress),
              _infoRow(
                context,
                label: 'Czas',
                value: _formatDateTime(record.createdAt),
              ),
              const SizedBox(height: 8),
              Align(
                alignment: Alignment.centerLeft,
                child: Text(
                  'Szczegóły',
                  style: Theme.of(
                    context,
                  ).textTheme.titleSmall?.copyWith(fontWeight: FontWeight.w600),
                ),
              ),
              const SizedBox(height: 8),
              if (record.details.isEmpty)
                Align(
                  alignment: Alignment.centerLeft,
                  child: Text(
                    'Brak dodatkowych danych.',
                    style: Theme.of(context).textTheme.bodyMedium,
                  ),
                )
              else
                ...record.details.entries.map(
                  (entry) => _infoRow(
                    context,
                    label: entry.key,
                    value: entry.value?.toString() ?? 'null',
                  ),
                ),
            ],
          ),
        );
      },
    );
  }
}
