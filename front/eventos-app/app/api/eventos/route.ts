import { NextResponse } from "next/server";

const BACKEND_EVENTOS_URL =
  process.env.EVENTOS_API_URL ?? "http://localhost:5295/api/evento";

type Zona = {
  nombre: string;
  precio: number;
  capacidad: number;
};

type Evento = {
  nombre: string;
  fecha: string;
  lugar: string;
  zonas: Zona[];
};

function isValidZona(zona: unknown): zona is Zona {
  if (!zona || typeof zona !== "object") {
    return false;
  }

  const candidate = zona as Record<string, unknown>;

  return (
    typeof candidate.nombre === "string" &&
    candidate.nombre.trim().length > 0 &&
    typeof candidate.precio === "number" &&
    Number.isFinite(candidate.precio) &&
    candidate.precio > 0 &&
    typeof candidate.capacidad === "number" &&
    Number.isInteger(candidate.capacidad) &&
    candidate.capacidad > 0
  );
}

function isValidEvento(payload: unknown): payload is Evento {
  if (!payload || typeof payload !== "object") {
    return false;
  }

  const candidate = payload as Record<string, unknown>;

  return (
    typeof candidate.nombre === "string" &&
    candidate.nombre.trim().length > 0 &&
    typeof candidate.fecha === "string" &&
    !Number.isNaN(Date.parse(candidate.fecha)) &&
    typeof candidate.lugar === "string" &&
    candidate.lugar.trim().length > 0 &&
    Array.isArray(candidate.zonas) &&
    candidate.zonas.length > 0 &&
    candidate.zonas.every(isValidZona)
  );
}

export async function POST(request: Request) {
  let payload: unknown;

  try {
    payload = await request.json();
  } catch {
    return NextResponse.json(
      {
        ok: false,
        message: "El cuerpo de la solicitud no es JSON valido.",
      },
      { status: 400 },
    );
  }

  if (!isValidEvento(payload)) {
    return NextResponse.json(
      {
        ok: false,
        message: "Payload invalido para crear el evento.",
      },
      { status: 400 },
    );
  }

  try {
    console.log(`Llamando al API externo '${BACKEND_EVENTOS_URL}' con el siguiente payload:`, JSON.stringify(payload));
    const upstreamResponse = await fetch(BACKEND_EVENTOS_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
      cache: "no-store",
    });

    const contentType = upstreamResponse.headers.get("content-type") ?? "";

    if (contentType.includes("application/json")) {
      const upstreamJson = (await upstreamResponse.json()) as Record<string, unknown>;

      if (!upstreamResponse.ok) {
        return NextResponse.json(
          {
            ok: false,
            message:
              typeof upstreamJson.message === "string"
                ? upstreamJson.message
                : "El API externo rechazo la solicitud.",
            details: upstreamJson,
          },
          { status: upstreamResponse.status },
        );
      }

      return NextResponse.json(upstreamJson, { status: upstreamResponse.status });
    }

    const upstreamText = await upstreamResponse.text();

    if (!upstreamResponse.ok) {
      return NextResponse.json(
        {
          ok: false,
          message: "El API externo devolvio una respuesta no valida.",
          details: upstreamText,
        },
        { status: upstreamResponse.status },
      );
    }

    return NextResponse.json(
      {
        ok: true,
        message: "Evento enviado correctamente al API.",
        data: payload,
      },
      { status: 201 },
    );
  } catch (error) {
    console.error("Error al conectar con el API externo:", error);
    return NextResponse.json(
      {
        ok: false,
        message: "No se pudo conectar con el API externo.",
        details: error instanceof Error ? error.message : "Error desconocido.",
      },
      { status: 502 },
    );
  }
}