import 'package:flutter/material.dart';
import 'package:frontend/Models/conversation.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Services/conversation_service.dart';
import 'package:go_router/go_router.dart';

class MyConversationsPage extends StatefulWidget {
  const MyConversationsPage({super.key});

  @override
  State<MyConversationsPage> createState() => _MyConversationsPageState();
}

class _MyConversationsPageState extends State<MyConversationsPage> {
  final _conversationService = ConversationService();
  late Future<List<Conversation>> _futureConverations;

  @override
  void initState() {
    super.initState();
    _futureConverations = _conversationService.getMyConversations();
  }

  void _reload() {
    setState(() {
      _futureConverations = _conversationService.getMyConversations();
    });
  }

  String _formatDate(DateTime date) {
    final d = date.day.toString().padLeft(2, '0');
    final m = date.month.toString().padLeft(2, '0');

    return '$d.$m.${date.year}';
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: MainAppBar(),
      body: FutureBuilder(
        future: _futureConverations,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }

          if (snapshot.hasError) {
            return Center(
              child: ElevatedButton.icon(
                onPressed: _reload,
                icon: const Icon(Icons.refresh),
                label: const Text('Wystąpił błąd! Spróbuj ponownie'),
              ),
            );
          }

          final conversations = snapshot.data ?? [];

          if (conversations.isEmpty) {
            return const Center(child: Text('Brak wiadomości'));
          }

          return ListView.separated(
            itemBuilder: (context, index) {
              final conversation = conversations[index];
              return ListTile(
                leading: const CircleAvatar(
                  child: Icon(Icons.chat_bubble_outline),
                ),
                title: Text(
                  'Oferta #${conversation.offerId}',
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                ),
                subtitle: Text(
                  conversation.lastMessage,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                ),
                trailing: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      _formatDate(conversation.lastMessageAt),
                      style: const TextStyle(
                        fontSize: 11,
                        color: Color.fromARGB(255, 68, 67, 67),
                      ),
                    ),
                    if (conversation.unreadCount > 0)
                      CircleAvatar(
                        radius: 10,
                        child: Text(
                          conversation.unreadCount.toString(),
                          style: const TextStyle(
                            fontSize: 11,
                            color: Colors.redAccent,
                          ),
                        ),
                      ),
                  ],
                ),
                onTap: () {
                  context.push('/conversations/${conversation.id}');
                },
              );
            },
            separatorBuilder: (_, __) => const Divider(height: 1),
            itemCount: conversations.length,
          );
        },
      ),
    );
  }
}
