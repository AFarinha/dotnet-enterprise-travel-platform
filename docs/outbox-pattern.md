# Outbox Pattern

The outbox will store integration-style internal events in the same transaction as the state change that produced them.

Future phases will add message states, retry metadata and worker processing.
