import 'package:flutter/material.dart';
import 'package:frontend/Models/activity_record.dart';
import 'package:frontend/Presentation/Widgets/admin_activity_section.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Services/admin_service.dart';

class AdminUserActivityPage extends StatefulWidget {
  final int userId;

  const AdminUserActivityPage({super.key, required this.userId});

  @override
  State<AdminUserActivityPage> createState() => _AdminUserActivityPageState();
}

class _AdminUserActivityPageState extends State<AdminUserActivityPage> {
  DateTime? _from;
  DateTime? _to;
  late Future<List<ActivityRecord>> _futureActivities;

  @override
  void initState() {
    super.initState();
    _futureActivities = _loadActivities();
  }

  Future<List<ActivityRecord>> _loadActivities() {
    return AdminService().getUserActivity(widget.userId, from: _from, to: _to);
  }

  //   Future<List<ActivityRecord>> _loadActivities() {
  //   return Future.value([
  //     ActivityRecord(
  //       id: 1,
  //       userId: widget.userId,
  //       offerId: null,
  //       action: 'LOGIN',
  //       details: {
  //         'message': 'User logged in',
  //       },
  //       ipAddress: '127.0.0.1',
  //       createdAt: DateTime.now(),
  //     ),
  //     ActivityRecord(
  //       id: 2,
  //       userId: widget.userId,
  //       offerId: 15,
  //       action: 'VIEW',
  //       details: {
  //         'message': 'Viewed offer #15',
  //       },
  //       ipAddress: '127.0.0.1',
  //       createdAt: DateTime.now(),
  //     ),
  //   ]);
  // }
  void _refresh() {
    setState(() {
      _futureActivities = _loadActivities();
    });
  }

  String _dateLabel(DateTime? dateTime, String fallback) {
    if (dateTime == null) {
      return fallback;
    }
    final local = dateTime.toLocal().toIso8601String();
    return local.substring(0, 10);
  }

  Future<void> _pickFromDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _from ?? DateTime.now(),
      firstDate: DateTime(2000),
      lastDate: DateTime(2100),
    );

    if (picked == null) return;

    setState(() {
      _from = DateTime(picked.year, picked.month, picked.day);
    });
  }

  Future<void> _pickToDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _to ?? DateTime.now(),
      firstDate: DateTime(2000),
      lastDate: DateTime(2100),
    );

    if (picked == null) return;

    setState(() {
      _to = DateTime(picked.year, picked.month, picked.day, 23, 59, 59);
    });
  }

  void _clearFilters() {
    setState(() {
      _from = null;
      _to = null;
      _futureActivities = _loadActivities();
    });
  }

  Widget _buildFilters() {
    return Wrap(
      spacing: 12,
      runSpacing: 12,
      crossAxisAlignment: WrapCrossAlignment.center,
      children: [
        OutlinedButton.icon(
          onPressed: _pickFromDate,
          icon: const Icon(Icons.date_range),
          label: Text('Od: ${_dateLabel(_from, 'brak')}'),
        ),
        OutlinedButton.icon(
          onPressed: _pickToDate,
          icon: const Icon(Icons.date_range),
          label: Text('Do: ${_dateLabel(_to, 'brak')}'),
        ),
        TextButton.icon(
          onPressed: _clearFilters,
          icon: const Icon(Icons.clear),
          label: const Text('Wyczyść'),
        ),
        ElevatedButton.icon(
          onPressed: _refresh,
          icon: const Icon(Icons.refresh),
          label: const Text('Odśwież'),
        ),
      ],
    );
  }

  Widget _buildContent() {
    return FutureBuilder<List<ActivityRecord>>(
      future: _futureActivities,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const Center(child: CircularProgressIndicator());
        }

        if (snapshot.hasError) {
          return Center(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(snapshot.error.toString(), textAlign: TextAlign.center),
                const SizedBox(height: 16),
                ElevatedButton(
                  onPressed: _refresh,
                  child: const Text('Spróbuj ponownie'),
                ),
              ],
            ),
          );
        }

        final records = snapshot.data ?? [];

        return AdminActivitySection(
          records: records,
          emptyMessage: 'Brak aktywności dla tego użytkownika.',
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      drawer: const MainSideMenu(),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Text(
              'Aktywność użytkownika #${widget.userId}',
              style: Theme.of(context).textTheme.headlineSmall,
            ),
            const SizedBox(height: 8),
            Text(
              'Filtry dat są opcjonalne.',
              style: Theme.of(context).textTheme.bodyMedium,
            ),
            const SizedBox(height: 16),
            _buildFilters(),
            const SizedBox(height: 16),
            Expanded(child: _buildContent()),
          ],
        ),
      ),
    );
  }
}
