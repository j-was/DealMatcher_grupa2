import 'package:frontend/Models/message.dart';
import 'package:signalr_netcore/signalr_client.dart';

class ConversationRealtimeService {
  HubConnection? _connection;

  Future<void> connect({
    required String baseUrl,
    required String token,
    required int conversationId,
    required void Function(Message message) onMessage,
  }) async {
    _connection = HubConnectionBuilder()
        .withUrl(
          '$baseUrl/hubs/conversations',
          options: HttpConnectionOptions(accessTokenFactory: () async => token),
        )
        .withAutomaticReconnect()
        .build();

    _connection!.on('message.created', (args) {
      if (args == null || args.isEmpty) {
        return;
      }

      final json = Map<String, dynamic>.from(args[0] as Map);
      final message = Message.fromJson(json);

      onMessage(message);
    });

    await _connection!.start();
    await _connection!.invoke('JoinConversation', args: [conversationId]);
  }

  Future<void> typing(int conversationId) async {
    await _connection!.invoke('Typing', args: [conversationId]);
  }

  Future<void> disconnect(int conversationId) async {
    await _connection!.invoke('LeaveConversation', args: [conversationId]);

    await _connection?.stop();
    _connection = null;
  }
}
