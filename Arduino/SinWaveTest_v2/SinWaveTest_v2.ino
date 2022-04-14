#define Pi 3.141592653589793

uint32_t time = 0;
uint32_t elapse = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
}

void loop() {
  double val = 0.0;
  for(int deg = 0; deg < 360; deg++){
    double rad = deg / (180 / Pi);
    val = sin(rad);
    elapse = millis() - time;
    Serial.print(elapse);
    Serial.print(" ");
    Serial.println(val);
    delay(5);
    //delayMicroseconds(10);
  }
}
