shader_type spatial;

uniform float spike_intensity : hint_range(0.0, 2.0); // Сила выступов
uniform float noise_scale : hint_range(0.0, 10.0); // Масштаб шума
uniform float spike_speed : hint_range(0.0, 5.0); // Скорость изменения шума

// Простая функция для создания шума
float hash(vec3 p) {
    p = fract(p * 0.3183099 + 0.1);
    p *= 17.0;
    return fract(p.x * p.y * p.z * (p.x + p.y + p.z));
}

float noise(vec3 p) {
    vec3 i = floor(p);
    vec3 f = fract(p);
    f = f * f * (3.0 - 2.0 * f);

    return mix(
        mix(
            mix(hash(i), hash(i + vec3(1.0, 0.0, 0.0)), f.x),
            mix(hash(i + vec3(0.0, 1.0, 0.0)), hash(i + vec3(1.0, 1.0, 0.0)), f.x),
            f.y
        ),
        mix(
            mix(hash(i + vec3(0.0, 0.0, 1.0)), hash(i + vec3(1.0, 0.0, 1.0)), f.x),
            mix(hash(i + vec3(0.0, 1.0, 1.0)), hash(i + vec3(1.0, 1.0, 1.0)), f.x),
            f.y
        ),
        f.z
    );
}

void vertex() {
    // Позиция шума с учётом времени для анимации
    vec3 noise_position = VERTEX * noise_scale + vec3(TIME * spike_speed);
    
    // Генерация шума
    float displacement = noise(noise_position) * spike_intensity;

    // Смещение вдоль нормали
    VERTEX += NORMAL * displacement;
}
