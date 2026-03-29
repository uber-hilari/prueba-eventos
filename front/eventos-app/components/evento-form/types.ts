export type Zona = {
  nombre: string;
  precio: number;
  capacidad: number;
};

export type Evento = {
  nombre: string;
  fecha: string;
  lugar: string;
  zonas: Zona[];
};

export type EventoFormProps = {
  title?: string;
  description?: string;
  submitLabel?: string;
};

export type ZonaErrors = {
  nombre?: string;
  precio?: string;
  capacidad?: string;
};

export type EventoErrors = {
  nombre?: string;
  fecha?: string;
  lugar?: string;
  zonas: ZonaErrors[];
};

export type SubmitState = {
  status: "idle" | "success" | "error";
  message?: string;
};
