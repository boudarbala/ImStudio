# Legal Graph module

The `dotnet-mvc` branch now provides the initial domain foundation for extending ImStudio into a legal knowledge-graph workspace.

## Objective

Represent legal information as a graph while preserving the connection between graph objects and their authoritative source text.

```text
Law
 ├── Part
 │    ├── Chapter
 │    │    └── Article
 │    └── ...
 ├── AMENDS → Law
 ├── REPEALS → Law
 └── ...

Article
 ├── REFERS_TO → Article
 ├── CONCERNS → Concept
 └── INTERPRETED_BY → Case
```

## Initial domain model

`LegalNode` represents a graph node such as a law, article, case, concept, jurisdiction, offence or sanction.

`LegalRelation` represents a directed relationship between two legal nodes.

The model deliberately keeps `Source`, `Identifier`, `Language` and `Text` so that later retrieval and RAG features can maintain source traceability.

## Planned architecture

```text
Documents
   │
   ▼
Ingestion ──► Normalization ──► Legal Graph
                                  │
                    ┌─────────────┼─────────────┐
                    ▼             ▼             ▼
                 Vector        Keyword        Graph
                 Search        Search         Search
                    └─────────────┼─────────────┘
                                  ▼
                              Legal RAG
                                  │
                                  ▼
                         Source-grounded answer
```

## Target capabilities

1. Import PDF/HTML/XML legal sources.
2. Extract laws, sections and articles.
3. Build typed legal nodes and relations.
4. Preserve source references and document versions.
5. Support French and Arabic legal content.
6. Add keyword, vector and graph retrieval.
7. Add GraphRAG-style question answering with citations to source articles.
8. Support Neo4j or another graph database without coupling the domain model to one vendor.
9. Add an interactive graph/editor view to the ImStudio web interface.

## Legal relation vocabulary

The first relation vocabulary includes:

- `REFERS_TO`
- `AMENDS`
- `REPEALS`
- `CONCERNS`
- `INTERPRETED_BY`
- `DERIVED_FROM`
- `APPLIES_TO`
- `SANCTIONS`

The vocabulary is intentionally extensible and should eventually be represented by an explicit ontology.

## Scope

This module is a software/data-model foundation. It does not itself provide legal advice or determine the legal validity of a source. Production deployments should preserve provenance, publication/version dates and the authoritative source for every legal assertion.
