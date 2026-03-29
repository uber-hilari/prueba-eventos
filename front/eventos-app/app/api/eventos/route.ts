import { NextResponse } from "next/server";

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
  const payload = await request.json();

  if (!isValidEvento(payload)) {
    return NextResponse.json(
      {
        ok: false,
        message: "Payload invalido para crear el evento.",
      },
      { status: 400 },
    );
  }

  return NextResponse.json(
    {
      ok: true,
      message: "Evento recibido correctamente.",
      data: payload,
    },
    { status: 201 },
  );
}